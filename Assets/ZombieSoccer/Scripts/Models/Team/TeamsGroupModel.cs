using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.Models.DeckM;

namespace ZombieSoccer.Models.TeamM
{
    public class TeamsGroupModel
    {

        #region Injects

        [Inject]
        DeckModel deckModel;

        [Inject]
        public TeamPositionsPresetsCollection PositionsPresets { get; set; }

        #endregion

        #region Fields

        public List<TeamSetup> TeamsList { get; set; } = new List<TeamSetup>();

        public TeamSetup Team => TeamsList[CurrentTeamIndex];

        public int[] TeamPositionPreset => PositionsPresets.teamPositions[Team.presetIndex].charactersPerSegment;

        public int CurrentTeamIndex { get; set; } = 0;

        #endregion

        #region mock

        // mock json
        public string MockGetJson()
        {
            var characters = new List<Character>( deckModel.Characters.data.Values);
            int ground = characters.Count < 7 ? characters.Count : 7;
            TeamSetup teamSetup = new TeamSetup();
            teamSetup.presetIndex = 0;
            for (int i = 0; i < ground; i++)
                teamSetup.characters[i] = characters[i].instanceId;
            TeamsList.Add(teamSetup);
            return JsonConvert.SerializeObject(TeamsList);
        }

        #endregion        

        public TeamsGroupModel( string arg )
        {            
            Setup();
        }

        public async void Setup()
        {            
            while( deckModel == null || deckModel.Characters.data.Count == 0)            
                await Task.Yield();

            TeamsList = JsonConvert.DeserializeObject<List<TeamSetup>>( MockGetJson() );
            CurrentTeamIndex = 0;

            if( 0 == TeamsList.Count )
                TeamsList.Add(new TeamSetup());
            Validate();
        }

        public void ChangePositions(int from, int to)
        {
            string _from = Team.characters[from];
            Team.characters[from] = Team.characters[to];
            Team.characters[to] = _from;
        }

        public void AddCharacter( int index , string instanceID )
        {
            Team.characters[index] = instanceID;
        }

        public void RemoveCharacter( int index)
        {
            Team.characters[index] = String.Empty;
        }

        public bool Validate()
        {
            bool result = true;
            int length = Team.characters.Length;
            for (int i = 0; i < length; i++)
            {
                var instanceId = TeamsList[CurrentTeamIndex].characters[i];
                if (string.IsNullOrEmpty(instanceId))
                {
                    result = false;
                    continue;
                }
                if (!deckModel.Characters.data.TryGetValue(instanceId, out var outCharacter))
                    if (null == outCharacter)
                    {
                        result = false;
                        TeamsList[CurrentTeamIndex].characters[i] = String.Empty;
                    }
            }
            return result;
        }
    }
}

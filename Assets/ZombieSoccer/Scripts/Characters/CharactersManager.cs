using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;
using ZombieSoccer.Database;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace ZombieSoccer.GameLayer.Characters
{
    [Serializable]
    public class CharacterPosition
    {
        public int x, y;
        public string characterID;

        public override string ToString()
        {
            return $"{characterID}, {x} : {y}";
        }
    }

    [System.Serializable]
    public class CharactersData
    {
        [SerializeField]
        public Dictionary<string, Character> people;
        [SerializeField]
        public Dictionary<string, Character> zombies;

        public Dictionary<string, Character> MakeCopy(Dictionary<string, Character> source)
        {
            Dictionary<string, Character> result = new Dictionary<string, Character>();
            foreach (var item in source)
            {
                result.Add(item.Key, item.Value.MakeCopy());
            }

            return result;
        }
    }

    public class CharactersManager : IInitializable
    {
        [Inject]
        private FirebaseAppFacade firebaseAppFacade;

        public bool IsComplete { get; private set; }

        //public const int MaxCharacterLevel = 50;

        UniTask uniTask;

        public UniTaskStatus status => uniTask.Status;

        public static DBStruct<CharactersData> DefaultCharacters { get; private set; }

        public async void Initialize()
        {
            while (!firebaseAppFacade.IsComplete)
                await Task.Yield();

            var path = $"{CommonStrings.DBTableDefaultCharactersPath}/";
            DefaultCharacters = new DBStruct<CharactersData>(path, firebaseAppFacade.FirebaseApp);

            await Task.Yield();
            IsComplete = true;
        }

        public static Dictionary<string, Character> MakeDefaultCharactersSet()
        {
            return DefaultCharacters.data.MakeCopy(DefaultCharacters.data.people);
        }

        public static Character FindArchitypeById(string characterId)
        {
            if (string.IsNullOrEmpty(characterId))
                return null;

            if (DefaultCharacters.data.people.ContainsKey(characterId))
                return DefaultCharacters.data.people[characterId];

            if (DefaultCharacters.data.zombies.ContainsKey(characterId))
                return DefaultCharacters.data.zombies[characterId];

            return null;
        }

        public static List<Character> GetRandomCharactersList(TeamType teamType, int count)
        {
            var result = new List<Character>();
            var targetTeam = (teamType == TeamType.Ally) ? DefaultCharacters.data.people : DefaultCharacters.data.zombies;

            Enumerable.Repeat(0, count).ToList().ForEach(x => result.Add(targetTeam.Values.ToList()[UnityEngine.Random.Range(0, targetTeam.Values.Count)]));

            return result;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.GameLayer.UI;
using ZombieSoccer.Models.DeckM;
using ZombieSoccer.Models.MatchM;
using ZombieSoccer.Models.TeamM;
using ZombieSoccer.ReactiveInput;
using ZombieSoccer.UI.Presenters;

namespace ZombieSoccer.UI.Views
{

    public class ViewPlacementCharacters : BasePagePresenter
    {
        #region Inject
        [Inject]
        DeckModel deckModel;
        
        [Inject]
        TeamsGroupModel teamsGroupModel;

        [Inject]
        DetailCharacterView.Factory detailCharacterViewFactory;

        [Inject]
        CharacterPositionPlaceholder.Factory characterPositionPlaceholderFactory;

        [Inject]
        SignalBus signalBus;

        [Inject]
        ObservableInputContextService inputContextService;

        [Inject]
        PageManager pageManager;        

        [Inject]
        UserModel userModel;

        [Inject]
        MainButtonElements mainButtonElements;
        #endregion

        #region Renderer elements
        [SerializeField]
        GameObject[] fieldSegments;

        [SerializeField]
        Transform mainButtonContainer;
        #endregion                

        private const string _nextPage = "UIView_MatchResult";

        private string _scenarioId;

        List<DetailCharacterView> characterViewsList = new List<DetailCharacterView>();
        List<CharacterPositionPlaceholder> characterPositionPlaceholders = new List<CharacterPositionPlaceholder>();

        public static ReactiveProperty<int> PresetIndex { get; set; } = new ReactiveProperty<int>();

        protected override void Enable()
        {
            base.Enable();
            
            PresetIndex.Subscribe(teamPresetIndex =>
            {
                UpdateField(teamPresetIndex);
            });
            signalBus.Subscribe<DragableUIVIewSignal>(DragableUIVIewSignalReceiver);
        }        

        protected override void Disable()
        {
            base.Disable();
            ReleaseFieldResources();
            signalBus.Unsubscribe<DragableUIVIewSignal>(DragableUIVIewSignalReceiver);
        }

        private void DragableUIVIewSignalReceiver()
        {
            UpdatePlayButton();
        }

        public void UpdateField(int teamPresetIndex = -1)
        {
            teamsGroupModel.Team.presetIndex = -1 == teamPresetIndex ? teamsGroupModel.Team.presetIndex : teamPresetIndex;
            ReleaseFieldResources();
            FillField();
        }

        private void FillField()
        {
            UpdatePlayButton();
            mainButtonElements.SetState(mainButtonContainer, teamsGroupModel.Validate());
            int[] presets = teamsGroupModel.TeamPositionPreset;
            string[] charactersID = teamsGroupModel.Team.characters;
            Dictionary<string, Character> characters = deckModel.Characters.data;
            int placeholderIndex = 0;
            for (int fieldIndex = -1; fieldIndex < presets.Length; fieldIndex++)
                for (int segmentIndexOnField = 0; segmentIndexOnField < (fieldIndex == -1 ? 1 : presets[fieldIndex]); segmentIndexOnField++, placeholderIndex++)
                    SetupSegment(placeholderIndex, 1 + fieldIndex);
        }

        private void SetupSegment(int placeholderIndex, int fieldIndex)
        {
            var placeholder = SetupCharacterPositionPlaceholder(placeholderIndex, fieldIndex);
            characterPositionPlaceholders.Add(placeholder);

            string characterID = teamsGroupModel.Team.characters[placeholderIndex];
            if(string.IsNullOrEmpty(characterID)) return;

            var view = SetupDetailCharacterView(placeholder, placeholderIndex, characterID);
            characterViewsList.Add(view);
            placeholder.SetObjectToTarget(view);
        }

        private CharacterPositionPlaceholder SetupCharacterPositionPlaceholder(int placeholderIndex, int fieldIndex)
        {
            
            var placeholder = characterPositionPlaceholderFactory.Create();
            placeholder.transform.SetParent(fieldSegments[fieldIndex].transform.GetChild(0).transform, false);
            placeholder.Index = placeholderIndex;
            var placeholderButton = placeholder.GetComponent<Button>();
            placeholderButton.onClick.RemoveAllListeners();
            placeholderButton.onClick.AddListener(() =>
               signalBus.Fire(new GridWidgetSignal() { GridWidgetSignalEnum = GridWidgetSignalEnum.Show, TargetTransform = placeholder.transform }));
            placeholder.ResetTarget();
            return placeholder;
        }

        private DetailCharacterView SetupDetailCharacterView(CharacterPositionPlaceholder placeholder, int placeholderIndex, string characterID)
        {
            var view = detailCharacterViewFactory.Create(deckModel.Characters.data[characterID], TeamType.Ally, null);
            view.transform.SetParent(placeholder.transform, false);
            view.transform.localPosition = Vector3.zero;
            var dragableUIView = view.gameObject.GetOrAddComponent<DragableUIView>();
            dragableUIView.SetInputContextService(inputContextService);
            dragableUIView.UpdateCurrentDragAndDropTarget();
            
            return view;
        }

        private void ReleaseFieldResources()
        {
            characterViewsList.ForEach(view => {
                view.Dispose();
                });
            characterViewsList.Clear();
            characterPositionPlaceholders.ForEach(placeholder =>
            {
                placeholder.Dispose();
            });
            characterPositionPlaceholders.Clear();
        }

        private void UpdatePlayButton()
        {
            mainButtonElements.SetState(mainButtonContainer, teamsGroupModel.Validate());
        }

        public void NextPage()
        {
            int[] teamPositionPreset = teamsGroupModel.TeamPositionPreset;
            string[] charactersID = teamsGroupModel.Team.characters;
            MatchMock(charactersID, teamPositionPreset);
        }

        private async void MatchMock(string[] selectedCharacters, int[] teamPositionsPresets)
        {
            var json = ConvertPresetsToJson(selectedCharacters, teamPositionsPresets);

            //  tmp
            List<string> scenariosId = new List<string>(userModel.Scenarios.data.Keys);
            string scenarioId = scenariosId[0];
            _scenarioId = scenarioId;
            //

            string post = String.Format(Web.POST_PlayMatchFormat, _scenarioId);
            string queryString = $"{Web.UrlToServer}/{post}";

            Debug.LogWarning($"[Match] Request queryString: {queryString}");

            var response = await Web.Request(Web.POST, queryString, Web.IdToken, json);

            Debug.LogWarning($"[Match] response: {response}");
            MatchOutcome outcome = JsonConvert.DeserializeObject<MatchOutcome>(response);

            pageManager.Fire(_nextPage, (System.Object)outcome);
        }

        private string ConvertPresetsToJson(string[] selectedCharacters, int[] teamPositionsPresets)
        {
            int idxCharacter = 0;
            int idxTeam = -1;

            TeamJson teamJson = new TeamJson();
            var roleByPosition = (RoleByPosition[])Enum.GetValues(typeof(RoleByPosition));

            foreach (var role in roleByPosition)
            {
                if (idxTeam < 0)
                {
                    teamJson.Team.Add(new TeamNode(selectedCharacters[idxCharacter++], role.ToString()));
                    idxTeam++;
                    continue;
                }
                for (int i = 0; i < teamPositionsPresets[idxTeam]; i++)
                {
                    teamJson.Team.Add(new TeamNode(selectedCharacters[idxCharacter++], role.ToString()));
                }
                idxTeam++;
            }

            string json = JsonConvert.SerializeObject(teamJson);
            return json;
        }
    }

}

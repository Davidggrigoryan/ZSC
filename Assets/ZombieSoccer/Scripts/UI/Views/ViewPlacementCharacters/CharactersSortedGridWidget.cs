using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Zenject;
using ZombieSoccer.Extensions;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.Models.DeckM;
using ZombieSoccer.Models.TeamM;
using ZombieSoccer.ReactiveInput;
using ZombieSoccer.UI.Views;
using ZombieSoccer.UI.Widget;
using ZombieSoccer.Utils.ListOfViewExtensions;

namespace ZombieSoccer.GameLayer.UI
{

    public enum GridWidgetSignalEnum
    {
        Show,
        Hide
    }

    public class GridWidgetSignal
    {
        public GridWidgetSignalEnum GridWidgetSignalEnum { get; set; }

        public Transform TargetTransform { get; set; }
    }


    public class CharactersSortedGridWidget : WidgetBase
    {

        #region Inject

        [Inject]
        DeckModel deckModel;

        [Inject]
        TeamsGroupModel teamsGroupModel;

        [Inject]
        DetailCharacterView.Factory detailCharacterViewFactory;

        [Inject]
        SignalBus signalBus;        

        [Inject]
        ObservableInputContextService inputContextService;
        #endregion

        #region UI Elements

        [SerializeField]
        private GridLayoutGroup gridLayoutGroup;
        

        [SerializeField]
        private Button gkButton, cbButton, cmButton, stButton, allButton;        
        
        private Transform targetParent;

        [SerializeField]
        private Color enableButtonColor, disableButtonColor;

        [SerializeField]
        private Color enableTextColor, disableTextColor;

        [SerializeField]
        Button closeButton;

        // Active\deactive root component the widget         
        [SerializeField]
        Transform root;

        [SerializeField]
        Transform filterButtonsPanel;

        #endregion        

        CharacterRole selectRole = CharacterRole.None;

        List<DetailCharacterView> charactersList = new List<DetailCharacterView>();
        
        protected override void Inititalize()
        {
            base.Inititalize();

            SetupFilterButtons();
            closeButton.onClick.AddListener(() => SendGridWidgetSignal(GridWidgetSignalEnum.Hide));
        }

        public async override void Enable()
        {            
            base.Enable();
            signalBus.Subscribe<SortSignal>(SortSignalReceiver);
            signalBus.Subscribe<GridWidgetSignal>(GridWidgetSignalReceiver);

            await OnEnableCoroutine();
        }

        public override void Disable()
        {
            base.Disable();
            signalBus.Unsubscribe<SortSignal>(SortSignalReceiver);
            signalBus.Unsubscribe<GridWidgetSignal>(GridWidgetSignalReceiver);
        }

        // set to first position content component with detail characters
        // in begin after enable parent object this component after fill him details to initialized him position on zero 
        private void SetToFirstPosition()
        {
            var y = gridLayoutGroup.GetComponent<RectTransform>().sizeDelta.y;
            gridLayoutGroup.GetComponent<RectTransform>().localPosition = new Vector2(0, -y / 2);
        }

        // The predicate uses in sort func for removing selected details on placement page
        private bool PredicateForRemoving(Character c)
        {   
            if (!teamsGroupModel.Team.characters.Contains(c.instanceId))
                return true;
            return false;
        }

        // Function for build the predicate function for filtre, uses in sort func
        private Predicate<Character> PredicateForRole(CharacterRole role)
        {            
            return (Character c) => role != CharacterRole.None ? c.role == role : true;
        }        

        private async void GridWidgetSignalReceiver( GridWidgetSignal gridWidgetSignal)
        {
            if (gridWidgetSignal.GridWidgetSignalEnum == GridWidgetSignalEnum.Show)
            {
                charactersList.ForEach(characterView => characterView.Dispose());
                targetParent = gridWidgetSignal.TargetTransform;

                root.gameObject.SetActive(true);

                SortByFilterFunc(selectRole);
                await UniTask.Yield();
                SetToFirstPosition();
            }
            else if(gridWidgetSignal.GridWidgetSignalEnum == GridWidgetSignalEnum.Hide)
            {
                root.gameObject.SetActive(false);
            }
        }

        private void SortByFilterFunc(CharacterRole role)
        {
            selectRole = role;
            deckModel.Sort<DefaultDeckSort>(PredicateForRemoving, PredicateForRole(role));            
        }

        private void SortSignalReceiver(SortSignal sortSignal)
        {
            charactersList.ForEach(characterView => characterView.Dispose());
            gridLayoutGroup.Prepare();
            charactersList = gridLayoutGroup.SpawnDetailCharacterViews(sortSignal.characters, detailCharacterViewFactory, ButtonHandler);
        }

        async UniTask OnEnableCoroutine()
        {
            await UniTask.Yield();

            gkButton.GetComponent<UIMeshRenderer>().GenerateMesh();
            cbButton.GetComponent<UIMeshRenderer>().GenerateMesh();
            cmButton.GetComponent<UIMeshRenderer>().GenerateMesh();            
            stButton.GetComponent<UIMeshRenderer>().GenerateMesh();
            allButton.GetComponent<UIMeshRenderer>().GenerateMesh();
        }        

        private void ButtonHandler( Character character )
        {
            SortByFilterFunc(selectRole);            
            teamsGroupModel.AddCharacter(targetParent.GetComponent<CharacterPositionPlaceholder>().Index, character.instanceId);
            GetComponentInParent<ViewPlacementCharacters>().UpdateField();
            signalBus.Fire(new GridWidgetSignal() { GridWidgetSignalEnum = GridWidgetSignalEnum.Hide });
        }

        private void SetupFilterButtons()
        {
            Action<Transform, CharacterRole> action = (Transform transform, CharacterRole role) => { SortByFilterFunc(role); FilterButtonsOutOfFocus(); FilterButtonFocus(transform); };
            action(allButton.transform, CharacterRole.None);

            gkButton.onClick.AddListener(() => { action(gkButton.transform, CharacterRole.GK); });
            cbButton.onClick.AddListener(() => { action(cbButton.transform, CharacterRole.CB); });
            cmButton.onClick.AddListener(() => { action(cmButton.transform, CharacterRole.CM); });
            stButton.onClick.AddListener(() => { action(stButton.transform, CharacterRole.ST); });
            allButton.onClick.AddListener(() => { action(allButton.transform, CharacterRole.None); });
        }

        private void FilterButtonFocus(Transform transform)
        {
            transform.GetComponentsInChildren<Image>()[1].color = enableButtonColor;
            transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = enableTextColor;
        }

        private void FilterButtonsOutOfFocus()
        {
            for(int i = 0; i < filterButtonsPanel.childCount; i++)
            {
                var transform = filterButtonsPanel.GetChild(i).transform;
                transform.GetComponentsInChildren<Image>()[1].color = disableButtonColor;
                transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = disableTextColor;
            }
        }

        public void SendGridWidgetSignal(GridWidgetSignalEnum gridWidgetSignalEnum)
        {
            signalBus.Fire(new GridWidgetSignal()
            {
                GridWidgetSignalEnum = gridWidgetSignalEnum
            });
        }
    }
}
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using System.Threading;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.Models.DeckM;
using ZombieSoccer.Rendering;
using ZombieSoccer.UI.Popups;
using ZombieSoccer.UI.Presenters;
using ZombieSoccer.UI.Utils;
using ZombieSoccer.UI.Widget;
using ZombieSoccer.Utils.IObservableExtensions;
using ZombieSoccer.Utitlies;

namespace ZombieSoccer.UI.Views
{
    public class ViewCharacterOverviewDetail : BasePagePresenter
    {
        #region Injects

        [Inject]
        UserModel user;

        [Inject]
        DiContainer container;

        [Inject]
        ResourcesManager resourcesManager;

        [Inject]
        RenderManager renderManager;

        [Inject]
        PageManager pageManager;

        [Inject]
        DeckModel deckModel;

        [Inject]
        CharacterAttributes characterAttributes;

        [Inject]
        TextRarityColors textRarityColors;

        [Inject]
        PopupBase<AboutPositionPopup>.Factory aboutPositionPopupFactory;
        
        [Inject]
        PopupBase<AboutRarityPopup>.Factory aboutRarityPopupFactory;
        
        [Inject]
        PopupBase<AboutElementsPopup>.Factory aboutElementsPopupFactory;

        [Inject]
        SignalBus signalBus;

        [Inject]
        MainButtonElements mainButtonElements;

        #endregion

        #region Visual datas        

        [SerializeField] Image background;

        [FoldoutGroup("Attributes")]
        [SerializeField] Image[] slotBackgrounds;

        [FoldoutGroup("Attributes")]
        [SerializeField] Image rarityIcon;

        [FoldoutGroup("Attributes")]
        [SerializeField] Image roleIcon;

        [FoldoutGroup("Attributes")]
        [SerializeField] Image animalIcon;

        [FoldoutGroup("Attributes")]
        [SerializeField] Image elementIcon;

        [FoldoutGroup("Attributes")]
        [SerializeField] TextMeshProUGUI lvlText;

        [SerializeField]
        Image lvlBackgroundIcon;

        [SerializeField]
        private TMPro.TextMeshProUGUI
             levelText,
             characterNameText;

        [FoldoutGroup("Attributes")] [ SerializeField]
        private TMPro.TextMeshProUGUI
             sumText,
             passText,
             receiveText,
             compText,
             mentalityText,
             hitsText,
             gkText;

        [FoldoutGroup("Rarity")] [SerializeField]
        private Image[] rarityStars;

        [FoldoutGroup("Upgrade prices")]
        [SerializeField]
        private TMPro.TextMeshProUGUI
            dustsUpgradePrice,
            elixirsUpgradePrice,
            crystalsUpgradePrice;

        [FoldoutGroup("Meta")] [SerializeField]
        Image animal, element;

        [SerializeField]
        Sprite starSprite, starPlaceholderSprite;

        [FoldoutGroup("Controls")] [SerializeField]
        private Button previousCharacter, nextCharacter, moveToMergePageButton, upgradeButton, scatterButton;

        [SerializeField]
        Transform characterContainer;

        [SerializeField]
        WidgetResources widgetCost;

        [SerializeField] Button positionAttributeButton;
        [SerializeField] Button rarityAttributeButton;
        [SerializeField] Button elementsAttributeButton;

        #endregion

        public int animSecondStateTimeWaiting = 5;

        private Transform prefabTargetTransform;
        private int animationDuration = 1;
        private bool _animationInProgress = false;
        private float _spineScale = 0f;
        private CancellationTokenSource _cancellationToken;

        private AboutPositionPopup _aboutPositionPopup;
        private AboutRarityPopup _aboutRarityPopup;
        private AboutElementsPopup _aboutElementsPopup;
        ReactiveProperty<Character> reactiveCharacter { get; set; } = new ReactiveProperty<Character>();
        
        private ReactiveProperty<Wallet> upgradePrice = new ReactiveProperty<Wallet>();

        protected override void Inititalize()
        {
            base.Inititalize();

            prefabTargetTransform = renderManager.ResolveLayer<RenderLayerSpine>().Container;

            _aboutPositionPopup ??= aboutPositionPopupFactory.Create(CommonStrings.AboutPositionPopup);
            _aboutRarityPopup ??= aboutRarityPopupFactory.Create(CommonStrings.AboutRarityPopup);
            _aboutElementsPopup ??= aboutElementsPopupFactory.Create(CommonStrings.AboutElementsPopup);

            positionAttributeButton.onClick.AsObservable().Subscribe( _ =>
            {
                signalBus.Fire(new AboutPositionPopupSignal()
                {
                    begin = (popup) =>
                    {
                        positionAttributeButton.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = popup.GetComponent<Canvas>().sortingOrder + 1;
                    },
                    end = ( ) =>
                    {
                        positionAttributeButton.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 2;
                    }
                });
            }).AddTo(disposables);

            rarityAttributeButton.onClick.AsObservable().Subscribe(_ => {
                signalBus.Fire(new AboutRarityPopupSignal()
                {
                    begin = (popup) =>
                    {
                        rarityAttributeButton.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = popup.GetComponent<Canvas>().sortingOrder + 1;
                    },
                    end = () =>
                    {
                        rarityAttributeButton.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 2;
                    }
                });
            }).AddTo(disposables);
            
            elementsAttributeButton.onClick.AsObservable().Subscribe(_ =>
            {
                signalBus.Fire(new AboutElementsPopupSignal()
                {
                    begin = (popup) =>
                    {
                        elementsAttributeButton.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = popup.GetComponent<Canvas>().sortingOrder + 1;
                    },
                    end = () =>
                    {
                        elementsAttributeButton.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 2;
                    }
                });
            }).AddTo(disposables);

            
        }

        protected override void Enable()
        {
            base.Enable();

            _cancellationToken = new CancellationTokenSource();
            previousCharacter.onClick.AsObservable().Subscribe(_ => ChangeCharacter(false)).AddTo(disposables);
            nextCharacter.onClick.AsObservable().Subscribe(_ => ChangeCharacter(true)).AddTo(disposables);
            characterContainer.gameObject.GetOrAddComponent<Button>().OnClickAsObservable().Subscribe(_ => 
                { _cancellationToken.Cancel(); _cancellationToken = new CancellationTokenSource(); Tap(); });

            upgradeButton.onClick.AsObservable()
                .BuildPipeline()
                .AddValidator (predicate: () => user.Wallet.data.ResourceSufficient(upgradePrice.Value),
                                   objectsOfObservation: upgradePrice
                                    )
                .AddAction(
                    action: () => { UpgardeCharacterLevel(); },
                    reactiveAction: () => mainButtonElements.SetState(upgradeButton.transform, true ) )
                .AddFailAction(
                    action: () => pageManager.FireToShopMovePopup(ForActionNotEnoughEnum.Dust),
                    reactiveAction: () => mainButtonElements.SetState(upgradeButton.transform, false, activityMapping: false) )
                .ToSubscribe()
                .AddTo(disposables);

            reactiveCharacter.Value = (Character)pageSignalHandler.args?[0];
            reactiveCharacter.Where(x => x != null).Subscribe(async x =>
            {
                characterAttributes.UpdateBackgroundOnRarity(background, (int)reactiveCharacter.Value.rarity);
                for(int i = 0; i < slotBackgrounds.Length; i++)
                    characterAttributes.UpdateSlotBackground(slotBackgrounds[i], (int)reactiveCharacter.Value.rarity);
                characterAttributes.UpdateCharacterRole(roleIcon, reactiveCharacter.Value.role);
                characterAttributes.UpdateIconOnRarity(rarityIcon, (int)reactiveCharacter.Value.rarity);
                characterAttributes.UpdateElement(elementIcon, reactiveCharacter.Value.element);
                characterAttributes.UpdateAnimal(animalIcon, reactiveCharacter.Value.animal);
                textRarityColors.UpdateTextColor(lvlText, (int)reactiveCharacter.Value.rarity);
                lvlBackgroundIcon.sprite = await resourcesManager.GetSprite("Profile/Level", CharacterAttributes.GetIndexForRarityElement((int)reactiveCharacter.Value.rarity).ToString());

                SetupGraphic(x);
                DOTween.To((arg) => UpdateStars((int)arg), 0, (float)x.rarity, animationDuration);
                
                UpdateAttributesInfo(x);
                UpdateUpgradePrice(x.instanceId);

                characterNameText.text = x.characterName;
                levelText.text = x.level.ToString();

                animal.sprite = resourcesManager.AnimalsIcons[x.animal];
                element.sprite = resourcesManager.ElementsIcons[x.element];

                SetupInfluenceButtons(x.rarity);
                
                await WaitingTimeForSecondAnimation();
            }).AddTo(disposables);            

            //Observable.EveryUpdate().Subscribe(_ => CorrectMeshSize()).AddTo(disposables);
        }

        private async void UpdateUpgradePrice(string instanceId)
        {
            string str = String.Format(Web.GET_UpgradePriceApi, instanceId);
            string api = $"{Web.UrlToServer}/{str}";

            upgradePrice.Value = await Web.Request<Wallet>(Web.GET, api, Web.IdToken);
            widgetCost.UpdateValues(upgradePrice.Value, user.Wallet.newData);

            //bool walletSufficient = user.Wallet.data.ResourceSufficient(upgradePrice);

            //upgradeButton.enabled = walletSufficient;
        }

        private void SetupInfluenceButtons( RarityEnum role )
        {
            moveToMergePageButton.gameObject.SetActive(false); scatterButton.gameObject.SetActive(false);

            if ( role == RarityEnum.Base )
                scatterButton.gameObject.SetActive(true);
            else
                moveToMergePageButton.gameObject.SetActive(true);
        }

        protected override void Disable()
        {
            base.Disable();
            _cancellationToken.Cancel();
            if (!IsInitialized)
                return;

            StopAllCoroutines();
            prefabTargetTransform.DestroyChilds();
        }

        [Button]
        public async void Tap()
        {
            var animator = prefabTargetTransform.GetComponentInChildren<Animator>();
            animator.SetTrigger("SecondAction");
            
            await WaitingTimeForSecondAnimation();            
        }

        private void ChangeCharacter(bool orderNext = true)
        {
            Debug.LogWarning($"[ViewCharacterOverviewDetail.ChangeCharacter]");
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
            var characters = deckModel.GetCharactersBySort<DefaultDeckSort>();
            var idx = characters.IndexOf(reactiveCharacter.Value) + (orderNext ? 1 : -1);
            reactiveCharacter.Value = (idx == characters.Count) ? characters.First() : (idx < 0) ? characters.Last() : characters[idx];
        }

        private void SetupGraphic(Character character)
        {
            prefabTargetTransform.DestroyChilds();
            var path = $"{CommonStrings.SpinePrefab}{character.portalName}/{character.characterName}/FullBodySpinePrefab";
            Debug.LogWarning($"[ViewCharacterOverviewDetail.SetupGraphic] Path: {path}");

            var spineFullBodySpineInstance = container.InstantiatePrefabResource(path);
            spineFullBodySpineInstance.transform.SetParent(prefabTargetTransform);
            spineFullBodySpineInstance.transform.localPosition = Vector3.zero;
            //spineFullBodySpineInstance.transform.localScale = prefabTargetTransform.localScale * 100;
        }
        
        async UniTask WaitingTimeForSecondAnimation()
        {            
            await UniTask.Delay(animSecondStateTimeWaiting*1000, false, PlayerLoopTiming.Update, _cancellationToken.Token);
            Tap();
        }        

        //private void CorrectMeshSize()
        //{
        //    if (prefabTargetTransform == null || prefabTargetTransform.childCount == 0)
        //        return;

        //    var meshTransform = prefabTargetTransform.GetChild(0).GetChild(0);

        //    float speed = 40f;
        //    float smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * speed);

        //    var mesh = prefabTargetTransform.GetComponentInChildren<MeshFilter>().mesh.bounds;

        //    float ratio = (mesh.size.y * 100) / (float)Screen.height;
        //    float targetSpine = 1.5f - ratio;

        //    meshTransform.localScale = new Vector3(_spineScale, _spineScale);
        //    _spineScale = Mathf.Lerp(_spineScale, targetSpine, smooth);

        //}

        private void UpdateAttributesInfo(Character character)
        {
            var passValue =
                   (character.passAttributes.beatValue
                    + character.passAttributes.upPassValue
                    + character.passAttributes.downPassValue) / 3f;

            var receiveValue =
                (character.passAttributes.upReceiveValue
                 + character.passAttributes.downReceiveValue) / 2f;

            var compValue =
                (character.competitionAttributes.fightValue
                 + character.competitionAttributes.interceptionValue
                 + character.competitionAttributes.dribbleValue
                 + character.competitionAttributes.pickingValue) / 4f;

            var mentalValue =
                (character.mentalityAttributes.assessmentValue
                 + character.mentalityAttributes.stabilityValue) / 2f;

            var hitsValue =
                 (character.hitAttributes.forceValue
                  + character.hitAttributes.accuracyValue
                  + character.hitAttributes.cunningValue) / 3f;

            var gkValue =
                (character.goalkeeperAttributes.ballProcessingValue
                 + character.goalkeeperAttributes.agilityValue
                 + character.goalkeeperAttributes.intuitionValue) / 3f;

            float sum = passValue + receiveValue + compValue + mentalValue + hitsValue + gkValue;

            int rarity = (int)reactiveCharacter.Value.rarity;            
            textRarityColors.UpdateTextColor(passText, rarity);
            textRarityColors.UpdateTextColor(receiveText, rarity);
            textRarityColors.UpdateTextColor(compText, rarity);
            textRarityColors.UpdateTextColor(mentalityText, rarity);
            textRarityColors.UpdateTextColor(hitsText, rarity);
            textRarityColors.UpdateTextColor(gkText, rarity);

            DOTween.To((arg) => { sumText.text = ((int)arg).ToString(); }, float.Parse(sumText.text), sum, animationDuration);
            DOTween.To((arg) => { passText.text = ((int)arg).ToString(); }, float.Parse(passText.text), passValue, animationDuration);
            DOTween.To((arg) => { receiveText.text = ((int)arg).ToString(); }, float.Parse(receiveText.text), receiveValue, animationDuration);
            DOTween.To((arg) => { compText.text = ((int)arg).ToString(); }, float.Parse(compText.text), compValue, animationDuration);
            DOTween.To((arg) => { mentalityText.text = ((int)arg).ToString(); }, float.Parse(mentalityText.text), mentalValue, animationDuration);
            DOTween.To((arg) => { hitsText.text = ((int)arg).ToString(); }, float.Parse(hitsText.text), hitsValue, animationDuration);
            DOTween.To((arg) => { gkText.text = ((int)arg).ToString(); }, float.Parse(gkText.text), gkValue, animationDuration);
        }
        private void SetZeroToTextComponents()
        {
            sumText.text = "0";
            passText.text = "0";
            receiveText.text = "0";
            compText.text = "0";
            mentalityText.text = "0";
            hitsText.text = "0";
            gkText.text = "0";
        }

        private void UpdateStars(int rarity)
        {
            foreach (var t in rarityStars)
                t.sprite = starPlaceholderSprite;

            for (int i = 0; i < rarity; i++)
            {
                rarityStars[i].sprite = starSprite;
            }
        }

        [Button]
        private void UpgardeCharacterLevel()
        {
            int lvl = reactiveCharacter.Value.level;
            Action upgradeAction = async () =>
            {
                var character = await deckModel.UpgradeCharacterLevel(reactiveCharacter.Value.instanceId);
                reactiveCharacter.Value = character;
            };
            if (0 == (lvl + 1) % 10)

                pageManager.FireUpgradeCharacterPopup(() =>
                {
                    upgradeAction.Invoke();
                });
            else
                upgradeAction.Invoke();
        }

        public void MoveToMergePage( )
        {            
            pageManager.Fire("UIView_CharacterUpdateRarity", (System.Object)reactiveCharacter.Value);
        }

        public void Scatter()
        {
            pageManager.FireDisintegratePopup(ScatterFunc, 300, 20);
        }

        private void ScatterFunc()
        {
            Action callback = () =>
            {
                var characterId = reactiveCharacter.Value.instanceId;
                string deleteApi = String.Format(Web.DELETE_CaracterFormat, characterId);
                string api = $"{Web.UrlToServer}/{deleteApi}";
                var response = Web.Request(Web.DELETE, api, Web.IdToken);
                Debug.Log($"Delete character, response: {response}");
                deckModel.Characters.data.Remove(characterId);
            };

            deckModel.Scatter(callback);

            pageManager.Fire("UIView_CharactersOverview");
        }
    }
}




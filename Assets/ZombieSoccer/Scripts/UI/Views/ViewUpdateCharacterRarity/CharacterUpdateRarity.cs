using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.CharactersMergeController;
using ZombieSoccer.Extensions;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.GameLayer.UI;
using ZombieSoccer.Localization;
using ZombieSoccer.UI;
using ZombieSoccer.UI.Presenters;

namespace ZombieSoccer
{
    public class CharacterUpdateRarity : BasePagePresenter
    {
        #region Inject

        [Inject]
        PageManager pageManager;

        [Inject]
        ResourcesManager resourceManager;
        
        [Inject]
        DetailCharacterView.Factory detailCharacterViewFactory;        

        [Inject]
        CharactersMerge charactersMerge;

        [Inject]
        CharacterAttributes characterAttributes;

        [Inject]
        MainButtonElements mainButtonElements;
        #endregion

        #region Fields

        ReactiveProperty<Character> _reactiveCharacter = new ReactiveProperty<Character>();

        ReactiveProperty<int> _countAdded = new ReactiveProperty<int>();

        Dictionary<String, DetailCharacterView> _prefabsMap = new Dictionary<string, DetailCharacterView>();

        String[] _selectedId = new String[3];

        int _slotIdx = 0;

        int _availableSlotCount = 2;        

        bool _mergingAvailable = false;

        Character _resultCharacter;

        #endregion

        #region UI Elements

        [SerializeField] GameObject rightCart, leftCart;        

        [SerializeField] GameObject currentCharacterCartPlace;

        [SerializeField] GameObject resultCart;

        [SerializeField] Transform gridTransform;

        [SerializeField] TMPro.TextMeshProUGUI levelText;

        [SerializeField] TMPro.TextMeshProUGUI characterNameText;

        [SerializeField] Image characterFullBody;

        [SerializeField] Image animal, element;

        [SerializeField] Image[] stars;

        [SerializeField] Sprite starSprite, starPlaceholderSprite;

        [SerializeField] Transform mainButtonContainer;

        [SerializeField] Sprite InMergeIcon;

        [SerializeField] Image Background;

        [SerializeField] Image RarityIcon;

        [SerializeField] Button BackButton;

        #endregion
        

        protected override void Inititalize()
        {
            base.Inititalize();

            _reactiveCharacter.Where(x => x != null).Subscribe(x =>
            {
                _selectedId[0] = x.instanceId;
            });

            mainButtonContainer.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            mainButtonContainer.GetComponentInChildren<Button>().onClick.AddListener(() => MergeFunc());

            _countAdded.Subscribe(_ =>
           {
               _mergingAvailable = CheckSlotsOnFill();

               if (_mergingAvailable)
                   MergingAvailable();
               else
                   MergingNotAvailable();
           });

            BackButton.GetComponent<Button>().onClick.RemoveAllListeners();
            BackButton.GetComponent<Button>().onClick.AddListener(() => pageManager.Fire("UIView_CharacterOverviewDetail", (System.Object)_reactiveCharacter.Value));
        }

        protected override void Enable()
        {
            base.Enable();
            
            _reactiveCharacter.Value = (Character)GetComponent<PageSignalHandler>()?.args?[0];

            _countAdded.Value = 0;
            _slotIdx = 0;

            CreateSelectedCharacterCart();
            SpawnCarts();
            EnableAvailableSlots(_reactiveCharacter.Value.rarity);
            characterAttributes.UpdateBackgroundOnRarity(Background, (int)_reactiveCharacter.Value.rarity);
            characterAttributes.UpdateIconOnRarity(RarityIcon, (int)_reactiveCharacter.Value.rarity);

            mainButtonElements.SetState(mainButtonContainer, false);
        }

        protected override void Disable()
        {
            base.Disable();
            ClearSequences();
        }
        
        private void MergingAvailable()
        {
            DeactivatingOtherCharacters();

            _resultCharacter = charactersMerge.Merge(_reactiveCharacter.Value.MakeCopy(), _prefabsMap.Select(x => x.Value.character).ToArray());
            SpawnResultCart( resultCart.transform, _resultCharacter );

            mainButtonElements.SetState(mainButtonContainer, true);
        }

        private void MergingNotAvailable()
        {
            ActivatingDeactivatedCharacters();
            DestroyChilds(resultCart.transform);

            mainButtonElements.SetState(mainButtonContainer, false);
        }

        private void EnableAvailableSlots(RarityEnum rare)
        {
            RarityEnum[] r = new RarityEnum[] { RarityEnum.Epic, RarityEnum.Legendary, RarityEnum.LegendaryPlus };
            if (r.Contains(rare))
            {
                rightCart.GetComponent<Image>().ChangeAlpha(0.25F);                
                _availableSlotCount = 1;
            }
            else
            {
                rightCart.GetComponent<Image>().ChangeAlpha(1F);
                _availableSlotCount = 2;
            }
        }

        private void DeactivatingOtherCharacters()
        {
            foreach (var view in _prefabsMap.Values)            
                if (!_selectedId.Contains(view.character.instanceId))
                {
                    view.ExtLayer2.SetActive(true);
                    view.ExtLayer2.GetComponent<Image>().sprite = null;
                    view.ExtLayer2.GetComponent<RectTransform>().localScale = new Vector3(1F, 1F, 1F);
                    view.ExtLayer2.GetComponent<Image>().color = Color.black;                    
                    view.ExtLayer2.GetComponent<Image>().ChangeAlpha(0.8F);
                }
        }

        private void ActivatingDeactivatedCharacters()
        {
            foreach (var view in _prefabsMap.Values)
                if (!_selectedId.Contains(view.character.instanceId))
                {
                    view.ExtLayer1.SetActive(false);
                    view.ExtLayer2.SetActive(false);
                }
        }

        private void ClearSequences()
        {
            foreach (var cart in _prefabsMap.Values)
            {
                Destroy(cart.gameObject);
            }
            _prefabsMap.Clear();
            Array.Clear(_selectedId, 0, _selectedId.Length);

            DestroyChilds(rightCart.transform);
            DestroyChilds(leftCart.transform);
            DestroyChilds(resultCart.transform);
            DestroyChilds(currentCharacterCartPlace.transform);
        }

        private void SpawnCarts()
        {
            string selectedArchetype = _reactiveCharacter.Value.archetypeId;
            var characters = charactersMerge.GetValidCharacters(_reactiveCharacter.Value);
            foreach(var item in characters)
            {
                
                if ( false == charactersMerge.IsValidForMerge(_reactiveCharacter.Value, item ))
                    continue;

                var view = detailCharacterViewFactory.Create(item, TeamType.Ally, gridTransform.transform);
                view.gameObject.GetOrAddComponent<Button>().onClick.AddListener(() =>
                {
                    CharacterAddingFunc( view, item );
                });
                view.GetComponent<RectTransform>().localScale = new Vector3(0.8F, 0.8F, 1);
                _prefabsMap.Add(item.instanceId, view);
            }
        }
        
        private void CreateSelectedCharacterCart()
        {
            var selectedCharacter = _reactiveCharacter.Value;
            
            levelText.text = (selectedCharacter.level).ToString();
            characterNameText.text = selectedCharacter.characterName;
            characterFullBody.sprite = resourceManager.GetCharacterSprites(selectedCharacter).FullBody;
            var selectedCharacterCart = detailCharacterViewFactory.Create(selectedCharacter, TeamType.Ally, currentCharacterCartPlace.transform);
            animal.sprite = Resources.Load<Sprite>(Path.Combine(CommonStrings.PathAnimalsIcons, selectedCharacter.animal.ToString()));
            element.sprite = Resources.Load<Sprite>(Path.Combine(CommonStrings.PathElementsIcons, selectedCharacter.element.ToString()));

            UpdateStars();
        }

        private void UpdateStars()
        {
            for (int i = 0; i < stars.Length; i++)
                stars[i].sprite = starPlaceholderSprite;
            for (int i = 0; i < (int)_reactiveCharacter.Value.rarity; i++)
                stars[i].sprite = starSprite;
        }

        private void CharacterAddingFunc( DetailCharacterView view, Character character )
        {
            if( _mergingAvailable ) return;

            _selectedId[ 1 + _slotIdx ] = character.instanceId;
            
            switch( _slotIdx )
            {
                case 0:
                    SpawnCraftCart( view, leftCart.transform, character, 1 + _slotIdx);
                    break;
                case 1:
                    SpawnCraftCart( view, rightCart.transform, character, 1 + _slotIdx);
                    break;
            }
            
            MarkingSelectedCart( character.instanceId );
            
            _slotIdx = _slotIdx == 0 ? 1 : 0;
            _countAdded.Value++;
        }

        private void MarkingSelectedCart( string instanceId )
        {
            _prefabsMap.TryGetValue(instanceId, out var selectCart);
            selectCart.ExtLayer1.SetActive(true);
            selectCart.ExtLayer2.SetActive(true);

            selectCart.ExtLayer1.GetComponent<Image>().sprite = InMergeIcon;
            selectCart.ExtLayer1.GetComponent<RectTransform>().localScale = new Vector3(1.8F, 1F, 1F);

            selectCart.ExtLayer2.GetComponent<Image>().color = Color.cyan;
            selectCart.ExtLayer2.GetComponent<Image>().ChangeAlpha(0.2F);                        
        }

        private bool CheckSlotsOnFill()
        {
            for (int i = 1; i < _availableSlotCount + 1; i++)
                if (String.IsNullOrEmpty(_selectedId[i]))
                        return false;

            return true;
        }

        private void SpawnCraftCart( DetailCharacterView rootView, Transform parent, Character character, int idx)
        {
            DestroyChilds(parent);

            var view = detailCharacterViewFactory.Create(character, TeamType.Ally, parent);

            Action DestroyingFunc = () =>
            {
                DestroyChilds(parent);
                _selectedId[idx] = null;
                _slotIdx = idx == 1 ? 0 : 1;
                _countAdded.Value--;
                
                rootView.GetComponent<Button>().onClick.RemoveAllListeners();
                rootView.GetComponent<Button>().onClick.AddListener(() => CharacterAddingFunc(rootView, character));
            };

            view.gameObject.GetOrAddComponent<Button>().onClick.RemoveAllListeners();
            view.gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                DestroyingFunc();
            });

            rootView.gameObject.GetOrAddComponent<Button>().onClick.RemoveAllListeners();
            rootView.gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                DestroyingFunc();
            });
            view.GetComponent<RectTransform>().localScale = new Vector3(0.6F, 0.6F, 1F);
        }

        private void SpawnResultCart( Transform parent, Character character)
        {
            var view = detailCharacterViewFactory.Create(character, TeamType.Ally, parent);
            
            foreach(var image in view.GetComponentsInChildren<Image>())
            {
                image.DOFade(0.1F, 0.8F).SetLoops(int.MaxValue, LoopType.Yoyo);
            }
            foreach (var text in view.GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.DOFade(0.1F, 0.8F).SetLoops(int.MaxValue, LoopType.Yoyo);
            }            
        }

        private void DestroyChilds(Transform transform)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

        private void MergeFunc()
        {
            if( !_mergingAvailable ) return;            
            
            (Character, Character) tuple = (_reactiveCharacter.Value, _resultCharacter);
            pageManager.Fire("UIView_MergeConfirmPage", (System.Object)tuple, (System.Object)_selectedId);
        }    

    }

    #region Enums

    public enum StringUpdateRarityCollectionEnum
    {
        MergeError
    }

    #endregion
       
}
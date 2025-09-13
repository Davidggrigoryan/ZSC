using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.Extensions;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.UI;

namespace ZombieSoccer.GameLayer.UI
{
    public class DetailCharacterView : MonoBehaviour, ICharacterView, IPoolable<Character, TeamType, UnityEngine.Transform, IMemoryPool>, IDisposable
    {
        #region Interface implement
        public Character character { get; set; }
        public TeamType teamType { get; set; }
        public Transform GetTransform() => transform;
        public GameObject GetGameObject() => gameObject;
        #endregion

        #region Character visual data
        [Inject]
        private ResourcesManager resourcesManager;

        [Inject]
        private ColorsPallete colorPallete;

        [SerializeField]
        private Image characterIcon;

        [SerializeField]
        private TextMeshProUGUI characterLevel, characterRole;

        [SerializeField]
        private Image characterAnimalIcon, characterElementIcon;

        [SerializeField]
        private List<Image> coloredByRarityElements;

        [SerializeField]
        private Transform rarityStarsRootTransform;

        [SerializeField]
        private float starsAnimationDuration = 1f;

        [SerializeField]
        private Sprite activeStarSprite, inactiveStarSprite;

        [SerializeField]
        public GameObject ExtLayer1, ExtLayer2;
        #endregion

        IMemoryPool _memoryPool;
        
        public void Construct(in Character character, in TeamType teamType)
        {
            this.character = character;
            this.teamType = teamType;

            gameObject.name = character.characterName;
            characterIcon.sprite = resourcesManager.GetCharacterSprites(character).Icon;
            characterLevel.text = character.level.ToString();
            characterRole.text = character.role.ToString();
            characterAnimalIcon.sprite = resourcesManager.AnimalsIcons[character.animal];
            characterElementIcon.sprite = resourcesManager.ElementsIcons[character.element];

            int rarityElementIdx = CharacterAttributes.GetIndexForRarityElement((int)character.rarity);
            coloredByRarityElements.ForEach(i => i.color = colorPallete.charactersRarityColors[ rarityElementIdx ]);

            // animate stars
            var stars = rarityStarsRootTransform.ChildsToList();
            DOTween.To((arg) =>
                stars.Where(x => stars.IndexOf(x) <= (int)arg - 1).ToList().ForEach(x => x.GetComponent<Image>().sprite = activeStarSprite),
                0,
                (float)character.rarity, starsAnimationDuration);

        }

        public void Dispose()
        {
            if(_memoryPool is null || rarityStarsRootTransform is null) return;
            DestroyImmediate(this.GetComponent<DragableUIView>());
            rarityStarsRootTransform.ChildsToList().ForEach(x => x.GetComponent<Image>().sprite = inactiveStarSprite);
            var button = GetComponent<Button>();
            if (button != null) button.onClick.RemoveAllListeners();
            _memoryPool.Despawn(this);
        }

        public void OnDespawned()
        {
            _memoryPool = null;
        }

        public void OnSpawned(Character character, TeamType teamType, UnityEngine.Transform targetTransform, IMemoryPool memoryPool)
        {
            Construct(character, teamType);
            _memoryPool = memoryPool;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.SetParent(targetTransform, false);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }

        #region Factory
        public class Factory : PlaceholderFactory<Character, TeamType, Transform, DetailCharacterView> { }        
        #endregion
    }
}

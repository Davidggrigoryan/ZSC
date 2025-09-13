using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.GameLayer.Characters;
using System.Collections.Generic;
using ZombieSoccer.UI;

namespace ZombieSoccer.GameLayer.UI
{
    public class MatchResultCharacterView : MonoBehaviour, ICharacterView
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
        private List<Image> coloredByRarityElements;

        #endregion

        [Inject]
        public void Construct(in Character character, TeamType teamType)
        {
            this.character = character;
            this.teamType = teamType;

            gameObject.name = character.characterName;
            characterIcon.sprite = resourcesManager.GetCharacterSprites(character).Icon;

            int rarityElementIdx = CharacterAttributes.GetIndexForRarityElement((int)character.rarity);
            coloredByRarityElements.ForEach(i => i.color = colorPallete.charactersRarityColors[ rarityElementIdx ]);
        }

        #region Factory
        public class Factory : PlaceholderFactory<string, Character, TeamType, int, Transform, MatchResultCharacterView>
        {
            readonly DiContainer _container;

            public Factory(DiContainer container)
            {
                _container = container;
            }

            public override MatchResultCharacterView Create(string prefabPath, Character character, TeamType teamType, int goalsCount, Transform parent = null)
            {
                var obj = _container.InstantiatePrefabResourceForComponent<MatchResultCharacterView>(prefabPath, new object[] { character, teamType });
                obj.GetTransform().SetParent(parent, false);
                obj.GetTransform().transform.localPosition = Vector3.zero;
                obj.GetTransform().transform.localScale = Vector3.one;

                return obj;
            }
        }
        #endregion
    }
}

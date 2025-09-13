using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.ApplicationLayer.Data;

namespace ZombieSoccer.GameLayer.UI
{
    public class MinimalCharacterView : MonoBehaviour, ICharacterView
    {
        public Character character { get; set; }

        [SerializeField]
        private Image characterIcon, characterBack;

        [Inject]
        ResourcesManager resourcesManager;

        public Sprite allyBackSprite, enemyBackSprite;

        public TeamType teamType { get; set; }

        //private CharacterViewVisualFX _characterViewVisualFX;

        //public CharacterViewVisualFX characterViewVisualFX { get => _characterViewVisualFX; set => _characterViewVisualFX = value; }

        public void SetCharacter(in Character _character, TeamType _teamType)
        {
            character = _character;
            characterIcon.sprite = resourcesManager.GetCharacterSprites(character).Icon;
            //character.characterView = this;
            gameObject.name = character.characterName;
            teamType = _teamType;

            characterBack.sprite = (teamType == TeamType.Ally) ? allyBackSprite : enemyBackSprite;

            //_characterViewVisualFX = GetComponent<CharacterViewVisualFX>();
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }


        public class Factory : PlaceholderFactory<string, MinimalCharacterView>
        {
        }


#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            UnityEditor.Handles.Label(transform.position, $"{character.role}\n{character.characterName}\n{teamType}");
        }
#endif
    }
}

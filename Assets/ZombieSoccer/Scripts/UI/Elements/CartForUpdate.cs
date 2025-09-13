using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.ApplicationLayer.Data;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;

namespace ZombieSoccer.GameLayer.UI
{
    public class CartForUpdate : MonoBehaviour, ICharacterView
    {

        public Character character { get; set; }
        private MatchStatsStructure CurrentMachStats;

        //private TextMeshProUGUI characterName;
        [SerializeField]
        private Transform characterNameTransform;

        [SerializeField]
        private Image characterIcon;

        [SerializeField]
        private TextMeshProUGUI characterLevel, role;

        public Transform CartSlot, TemporaryParent;

        [Inject]
        ResourcesManager resourcesManager;

        public TeamType teamType { get; set; }
        //public CharacterViewVisualFX characterViewVisualFX { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        [SerializeField]
        private Image animalIcon, elementIcon;
        public Vector3 StartPosition, TargetPosition;

        public bool IsTouched = false, IsPut = false;

        public void SetCharacter(in Character _selectedCharacter, TeamType _teamType)
        {
            character = _selectedCharacter;
            characterIcon.sprite = resourcesManager.GetCharacterSprites(_selectedCharacter).Icon;
            characterLevel.text = character.level.ToString();
            //character.characterView = this;
            gameObject.name = character.characterName;
            teamType = _teamType;
            role.text = character.role.ToString();

            GetComponent<CharacterViewGraphic>().UpdateStars((int)character.rarity);
            GetComponent<CharacterViewGraphic>().UpdateColors((int)character.rarity);

            animalIcon.sprite = resourcesManager.AnimalsIcons[_selectedCharacter.animal];
            elementIcon.sprite = resourcesManager.ElementsIcons[_selectedCharacter.element];
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void GetVariables()
        {
            StartPosition = gameObject.transform.position;
            IsTouched = false;
            IsPut = false;

        }

        public void TurnOnDragAndDrop()
        {
            EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();

            /*entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => entry.callback.AddListener((data) => { DragCart(); }));
            trigger.triggers.Add(entry);*/

            //entry.eventID = EventTriggerType.EndDrag;
            //entry.callback.AddListener((data) => entry.callback.AddListener((data) => { BackCart(); }));
            //trigger.triggers.Add(entry);
        }

        public void DragCart()
        {

                gameObject.GetComponent<RectTransform>().localPosition = Input.mousePosition;
                transform.SetParent(TemporaryParent);
        }

        public void BackCart()
        {
        if (IsPut == false)
        {
            Debug.Log("THIS NAME " + character.characterName);
            transform.SetParent(CartSlot, false);
            GetComponent<RectTransform>().localPosition = StartPosition;
        }
        }

        public class Factory : PlaceholderFactory<string, CartForUpdate>
        {

        }

        /*#if unity_editor
                void ondrawgizmos()
                {
                    unityeditor.handles.label(transform.position, $"{character.role}\n{character.charactername}\n{teamtype}");
                }


        #endif*/

    }
}

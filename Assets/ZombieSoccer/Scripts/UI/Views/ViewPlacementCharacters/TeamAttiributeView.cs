using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.Models.DeckM;
using ZombieSoccer.Models.TeamM;

namespace ZombieSoccer.GameLayer.UI
{
    public class TeamAttiributeView : MonoBehaviour
    {

        [Inject]
        SignalBus signalBus;

        [Inject]
        TeamsGroupModel teamsGroupModel;

        [Inject]
        DeckModel deckModel;

        [SerializeField]
        private TextMeshProUGUI
            sumText,
            passText,
            receiveText,
            compText,
            mentalityText,
            hitsText,
            gkText;

        [SerializeField]
        private Transform paramsTransform;

        [SerializeField]
        private bool isOpen;        

        private void Start()
        {
            paramsTransform.gameObject.SetActive(isOpen);
            GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => OnClick());
            UpdateData(deckModel.GetCharactersBySort<DefaultDeckSort>()
                .Where(character => teamsGroupModel.Team.characters.Contains(character.instanceId)).ToList());
        }

        private void OnEnable()
        {
            signalBus.Subscribe<DragableUIVIewSignal>(DragableUIVIewSignalReceiver);
            signalBus.Subscribe<GridWidgetSignal>(DragableUIVIewSignalReceiver);
        }        

        private void OnDisable()
        {
            signalBus.Unsubscribe<DragableUIVIewSignal>(DragableUIVIewSignalReceiver);
            signalBus.Unsubscribe<GridWidgetSignal>(DragableUIVIewSignalReceiver);
        }

        private void DragableUIVIewSignalReceiver()
        {
            UpdateData(deckModel.GetCharactersBySort<DefaultDeckSort>()
                .Where(character => teamsGroupModel.Team.characters.Contains(character.instanceId)).ToList());
        }        

        private void ResetValues()
        {
            sumText.text = "0";
            passText.text = "0";
            receiveText.text = "0";
            compText.text = "0";
            mentalityText.text = "0";
            hitsText.text = "0";
            gkText.text = "0";
        }

        public void OnClick()
        {
            isOpen = !isOpen;
            paramsTransform.gameObject.SetActive(isOpen);
        }

        //Пас - среднее значение между выбиванием низким и высоким пасом
        //Прием - среднее значение между низким и высоким приемом
        //Конкуренция за мяч - среднее значение между борьба, перехват, дриблинг, отбор
        //Ментальность - среднее между “оценка ситуации” и “стабильность”
        //Удары - среднее между сила, хитрость и точность
        //Вратарь - среднее значение между обработкой, ловкостью и интуицией

        [SerializeField]
        private bool useMean; // or sum

        public void UpdateData(List<Character> currentCharacters)
        {
            if (currentCharacters.Count == 0)
            {
                ResetValues();
                return;
            }

            float
                passValue= 0,
                receiveValue = 0,
                compValue = 0,
                mentalValue = 0,
                hitsValue = 0,
                gkValue = 0;

            foreach (var character in currentCharacters)
            {
                passValue +=
                    (character.passAttributes.beatValue 
                    + character.passAttributes.upPassValue 
                    + character.passAttributes.downPassValue) / 3f;

                receiveValue +=
                    (character.passAttributes.upReceiveValue
                    + character.passAttributes.downReceiveValue) / 2f;

                compValue +=
                    (character.competitionAttributes.fightValue
                    + character.competitionAttributes.interceptionValue
                    + character.competitionAttributes.dribbleValue
                    + character.competitionAttributes.pickingValue) / 4f;

                mentalValue +=
                    (character.mentalityAttributes.assessmentValue
                    + character.mentalityAttributes.stabilityValue) / 2f;

                hitsValue +=
                     (character.hitAttributes.forceValue
                     + character.hitAttributes.accuracyValue
                     + character.hitAttributes.cunningValue) / 3f;

                gkValue +=
                    (character.goalkeeperAttributes.ballProcessingValue
                    + character.goalkeeperAttributes.agilityValue
                    + character.goalkeeperAttributes.intuitionValue) / 3f;
            }

            if (useMean)
            {
                passValue /= (float)currentCharacters.Count;
                receiveValue /= (float)currentCharacters.Count;
                compValue /= (float)currentCharacters.Count;
                mentalValue /= (float)currentCharacters.Count;
                hitsValue /= (float)currentCharacters.Count;
                gkValue /= (float)currentCharacters.Count;
            }

            sumText.text = ((int)passValue + (int)receiveValue + (int)compValue + (int)mentalValue + (int)hitsValue + (int)gkValue).ToString();
            passText.text = ((int)passValue).ToString();
            receiveText.text = ((int)passValue).ToString();
            compText.text = ((int)compValue).ToString();
            mentalityText.text = ((int)mentalValue).ToString();
            hitsText.text = ((int)hitsValue).ToString();
            gkText.text = ((int)gkValue).ToString();
        }
    }
}

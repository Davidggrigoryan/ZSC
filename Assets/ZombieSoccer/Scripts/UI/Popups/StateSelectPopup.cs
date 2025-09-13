using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZombieSoccer.Localization;

namespace ZombieSoccer.UI.Popups
{
    public class StateSelectPopupSignal
    {
        public StateSelectPopupEnum stateSelectPopupEnum;
        public Action button1Action;
        public Action button2Action;
    }

    public enum StateSelectPopupEnum
    {
        QuitTheGame,
        RestartThisMatch,
        LeaveThistMatch
    }

    public class StateSelectPopup : PopupBase<StateSelectPopup>
    {

        [SerializeField] TextMeshProUGUI titleTextMesh, button1TextMesh, button2TextMesh;

        [SerializeField] Button button1;

        [SerializeField] Button button2;

        private const string _localizationAssetName = "StateSelectPopup";

        protected override void Enable()
        {
            base.Enable();
            SetupSignalReceiver<StateSelectPopupSignal>(StateSelectPopupSignalReceiver);
        }

        private void StateSelectPopupSignalReceiver(StateSelectPopupSignal signalPayload)
        {
            base.popup.Show();
            string key = signalPayload.stateSelectPopupEnum.ToString();
            titleTextMesh.Localize(_localizationAssetName, key + "Title");
            button1TextMesh.Localize(_localizationAssetName, key + "Button1");
            button2TextMesh.Localize(_localizationAssetName, key + "Button2");
            
            button1.onClick.RemoveAllListeners();
            button2.onClick.RemoveAllListeners();

            button1.onClick.AddListener(() =>
            {
                signalPayload.button1Action.Invoke();
                base.popup.Hide();
            });
            button2.onClick.AddListener(() =>
            {
                signalPayload.button2Action.Invoke();
                base.popup.Hide();
            });
        }
    }
}

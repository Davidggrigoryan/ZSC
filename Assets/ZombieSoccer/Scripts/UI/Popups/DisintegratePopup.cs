using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZombieSoccer.Localization;

namespace ZombieSoccer.UI.Popups
{
    public class DisintegratePopupSignal
    {
        public Action action;
        public int dust, elixir;
    }

    public class DisintegratePopup : PopupBase<DisintegratePopup>
    {

        [SerializeField] TextMeshProUGUI titleTextMesh, textMesh, acceptButtonTextMesh;

        [SerializeField] Button acceptButton;

        [SerializeField] Transform dustItem, elixirItem;

        private const string _localizationAssetName = "DisintegratePopup";

        protected override void Initialize()
        {
            base.Initialize();
            titleTextMesh.Localize(_localizationAssetName, "DisintegrateTitle");
            textMesh.Localize(_localizationAssetName, "DisintegrateText");
            acceptButtonTextMesh.Localize(_localizationAssetName, "DisintegrateButton");
        }

        protected override void Enable()
        {
            base.Enable();
            SetupSignalReceiver<DisintegratePopupSignal>(DisintegratePopupSignalReceiver);
        }

        private void DisintegratePopupSignalReceiver(DisintegratePopupSignal signalPayload)
        {
            base.popup.Show();
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() => { 
                signalPayload.action.Invoke();
                base.popup.Hide();
            });
            dustItem.GetComponentInChildren<TextMeshProUGUI>().text = signalPayload.dust.ToString();
            elixirItem.GetComponentInChildren<TextMeshProUGUI>().text = signalPayload.elixir.ToString();
        }                
    }
}

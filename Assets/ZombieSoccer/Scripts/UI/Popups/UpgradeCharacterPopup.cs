using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZombieSoccer.Localization;

namespace ZombieSoccer.UI.Popups
{

    public class UpgradeCharacterPopupSignal
    {
        public Action action;
    }

    public class UpgradeCharacterPopup : PopupBase<UpgradeCharacterPopup>
    {

        [SerializeField]
        TextMeshProUGUI titleTextMesh, upgradeButtonTextMesh;

        [SerializeField]
        Button upgradeButton;

        private const string _localizationAssetName = "UpgradeCharacterPopup";

        protected override void Enable()
        {
            base.Enable();
            SetupSignalReceiver<UpgradeCharacterPopupSignal>(UpgradeCharacterPopupSignalReceiver);
        }

        private void UpgradeCharacterPopupSignalReceiver(UpgradeCharacterPopupSignal signalPayload)
        {
            base.popup.Show();
            titleTextMesh.Localize(_localizationAssetName, "UpgradeCharacterTitle");
            upgradeButtonTextMesh.Localize(_localizationAssetName, "UpgradeButton");

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => { signalPayload.action.Invoke(); base.popup.Hide(); });
        }
    }
}

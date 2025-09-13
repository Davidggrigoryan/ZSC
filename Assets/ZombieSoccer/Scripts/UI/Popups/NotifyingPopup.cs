using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZombieSoccer.Localization;

namespace ZombieSoccer.UI.Popups
{
    public class NotifyingPopupSignal
    {
        public NotifyingPopupEnum notifyingPopupEnum;
    }
    
    public enum NotifyingPopupEnum
    {
        LimitReached,
        NotEnoughResources
    }

    public class NotifyingPopup : PopupBase<NotifyingPopup>
    {

        [SerializeField] TextMeshProUGUI titleMesh, textMesh, buttonTextMesh;

        [SerializeField] Button gotItButton;

        private const string _localizationAssetName = "NotifyingPopup";

        protected override void Enable()
        {
            base.Enable();
            SetupSignalReceiver<NotifyingPopupSignal>(NotifyingPopupSignalReceiver);
        }

        private void NotifyingPopupSignalReceiver(NotifyingPopupSignal signalPayload)
        {
            base.popup.Show();            
            string key = signalPayload.notifyingPopupEnum.ToString();
            titleMesh.Localize(_localizationAssetName, key + "Title");
            textMesh.Localize(_localizationAssetName, key + "Text");
            buttonTextMesh.Localize(_localizationAssetName, "GotItButton");
            gotItButton.onClick.RemoveAllListeners();
            gotItButton.onClick.AddListener(() => { base.popup.Hide(); });
        }
    }
}

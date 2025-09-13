using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.Localization;

namespace ZombieSoccer.UI.Popups
{
    public class StorePopupSignal
    {
        public ForActionNotEnoughEnum forActionNotEnoughEnum;
    }

    public class StorePopup : PopupBase<StorePopup>
    {

        [Inject]
        ResourcesManager resourcesManager;

        [SerializeField] TextMeshProUGUI titleTextMesh, mainTextMesh, toMoveButtonTextMesh;

        [SerializeField] Button toMoveButton;

        [SerializeField] Image itemIcon;

        private const string _localizationAssetName = "StorePopup";

        protected override void Enable()
        {
            base.Enable();
            SetupSignalReceiver<StorePopupSignal>(StorePopupSignalReceiver);
        }

        private async void StorePopupSignalReceiver(StorePopupSignal signalPayload)
        {
            base.popup.Show();
            string key = signalPayload.forActionNotEnoughEnum.ToString();
            titleTextMesh.Localize(_localizationAssetName, key + "Title");
            mainTextMesh.Localize(_localizationAssetName, key + "Text");
            toMoveButtonTextMesh.Localize(_localizationAssetName, "GotItButton");

            toMoveButton.onClick.RemoveAllListeners();
            toMoveButton.onClick.AddListener(() => { base.popup.Hide(); });
            itemIcon.sprite = await resourcesManager.GetSprite(CommonStrings.ResourceUnitsPath, key);
        }
    }
}

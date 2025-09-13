using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.Localization;

namespace ZombieSoccer.UI.Popups
{
    public class TradePopupSignal
    {
        public ForActionNotEnoughEnum forActionNotEnoughEnum;
        public int count;
    }    

    public class TradePopup : PopupBase<TradePopup>
    {

        [Inject]
        ResourcesManager resourcesManager;

        [SerializeField] TextMeshProUGUI titleTextMesh, tradeButtonTextMesh;

        [SerializeField] Button tradeButton;

        [SerializeField] Image itemIcon;

        [SerializeField] TextMeshProUGUI itemCount;

        private const string _localizationAssetName = "TradePopup";

        protected override void Enable()
        {
            base.Enable();
            SetupSignalReceiver<TradePopupSignal>(TradePopupSignalReceiver);
        }

        private async void TradePopupSignalReceiver(TradePopupSignal signalPayload)
        {
            base.popup.Show();
            string key = signalPayload.forActionNotEnoughEnum.ToString();
            titleTextMesh.Localize(_localizationAssetName, key + "Title");
            tradeButtonTextMesh.Localize(_localizationAssetName, "TradeButton");

            tradeButton.onClick.RemoveAllListeners();
            tradeButton.onClick.AddListener(() => { base.popup.Hide(); });
            itemIcon.sprite = await resourcesManager.GetSprite(CommonStrings.ResourceUnitsPath, key);
            itemCount.text = signalPayload.count.ToString();
        }
    }
}

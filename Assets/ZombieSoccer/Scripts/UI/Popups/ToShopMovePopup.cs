using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.Localization;

namespace ZombieSoccer.UI.Popups
{
    public class ToShopMovePopupSignal
    {
        public ForActionNotEnoughEnum forActionNotEnoughEnum;
    }
    
    public enum ForActionNotEnoughEnum
    {
        Crystal = 0,
        Dust = 1,
        Slot = 30
    }

    public class ToShopMovePopup : PopupBase<ToShopMovePopup> 
    {

        [Inject]
        ResourcesManager resourcesManager;

        [SerializeField] TextMeshProUGUI titleTextMesh, mainTextMesh, toMoveButtonTextMesh;

        [SerializeField] Button toMoveButton;

        [SerializeField] Image itemIcon;

        private const string _localizationAssetName = "ToShopMovePopup";
        
        protected override void Enable()
        {
            base.Enable();
            SetupSignalReceiver<ToShopMovePopupSignal>(ToShopMovePopupSignalReceiver);
        }

        private async void ToShopMovePopupSignalReceiver(ToShopMovePopupSignal signalPayload)
        {
            base.popup.Show();
            string key = signalPayload.forActionNotEnoughEnum.ToString();
            titleTextMesh.Localize(_localizationAssetName, key + "Title");
            mainTextMesh.Localize(_localizationAssetName, key + "Text");
            toMoveButtonTextMesh.Localize(_localizationAssetName, "ToShopMoveButton");

            toMoveButton.onClick.RemoveAllListeners();
            toMoveButton.onClick.AddListener(() => { base.popup.Hide(); });
            itemIcon.sprite = await resourcesManager.GetSprite(CommonStrings.ResourceUnitsPath, GetItemName(signalPayload.forActionNotEnoughEnum));
        }

        private string GetItemName(ForActionNotEnoughEnum forActionNotEnoughEnum)
        {
            if ((int)forActionNotEnoughEnum < 30)
                return "Box" + forActionNotEnoughEnum.ToString();
            return forActionNotEnoughEnum.ToString();
        }
    }
}

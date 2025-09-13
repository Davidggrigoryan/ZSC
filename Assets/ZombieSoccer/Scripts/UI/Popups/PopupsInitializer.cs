using Zenject;

namespace ZombieSoccer.UI.Popups
{
    public class PopupsInitializer : IInitializable
    {
        [Inject]
        PopupBase<NotifyingPopup>.Factory notifyingPopupFactory;
        [Inject]
        PopupBase<DisintegratePopup>.Factory disintegratePopupFactory;
        [Inject]
        PopupBase<StateSelectPopup>.Factory stateSelectPopupFactory;
        [Inject]
        PopupBase<ToShopMovePopup>.Factory toShopMovePopupFactory;
        [Inject]
        PopupBase<UpgradeCharacterPopup>.Factory upgrageCharacterPopupFactory;

        public NotifyingPopup notifyingPopup { get; private set; }
        public DisintegratePopup disintegratePopup { get; private set; }
        public StateSelectPopup stateSelectPopup { get; private set; }
        public ToShopMovePopup toShopMovePopup { get; private set; }
        public UpgradeCharacterPopup upgradeCharacterPopup { get; private set; }

        public void Initialize()
        {
            notifyingPopup = notifyingPopupFactory.Create(CommonStrings.PrefabNotifyingPopup);
            disintegratePopup = disintegratePopupFactory.Create(CommonStrings.DisintegratePopup);
            toShopMovePopup = toShopMovePopupFactory.Create(CommonStrings.ToShopMovePopup);
            upgradeCharacterPopup = upgrageCharacterPopupFactory.Create(CommonStrings.UpgradCharacterPopup);
            stateSelectPopup = stateSelectPopupFactory.Create(CommonStrings.StateSelectPopup);
        }
    }
}

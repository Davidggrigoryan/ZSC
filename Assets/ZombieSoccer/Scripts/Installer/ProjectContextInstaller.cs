using Zenject;
using ZombieSoccer;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.CharactersMergeController;
using ZombieSoccer.Database;
using ZombieSoccer.DeepLink;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.GameLayer.UI;
using ZombieSoccer.Localization;
using ZombieSoccer.Models.DeckM;
using ZombieSoccer.ReactiveInput;
using ZombieSoccer.Rendering;
using ZombieSoccer.UI;
using ZombieSoccer.UI.Popups;
using ZombieSoccer.UI.ViewPortalM;
using ZombieSoccer.Utils.LocalStorage;
using ZombieSoccer.ZombieSoccer.Scripts.Locations.Pin;
using ZombieSoccer.ZombieSoccer.Scripts.MessageHandlers.GameFlowNS;
using ZombieSoccer.ZombieSoccer.Scripts.Models.Messages.GameFlowNS;
using ZombieSoccer.ZombieSoccer.Scripts.Models.Messages.MapsWidget;

public class ProjectContextInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<LocalStorage>().AsSingle().NonLazy();
        Container.Bind<TeamPositionsPresetsCollection>().FromScriptableObjectResource(CommonStrings.PalleteTeamPositionsPath).AsSingle().NonLazy();
        Container.Bind<ColorsPallete>().FromScriptableObjectResource(CommonStrings.PalleteColorsPath).AsSingle().NonLazy();
        Container.Bind<TextRarityColors>().FromScriptableObjectResource(CommonStrings.TextRarityColors).AsSingle().NonLazy();
        Container.Bind<ShieldPalleteScriptableObject>().FromScriptableObjectResource(CommonStrings.PalleteShieldPath).AsSingle().NonLazy();
        Container.Bind<PageManagerConfigScriptableObject>().FromScriptableObjectResource(CommonStrings.PageManagerConfig).AsSingle().NonLazy();

        Container.Bind<CharacterAttributes>().FromScriptableObjectResource(CommonStrings.CharacterAttributesPath).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ObservableInputContextService>().AsSingle().NonLazy();
        Container.Bind<MainButtonElements>().FromScriptableObjectResource(CommonStrings.MainButtonElementsPath).AsSingle().NonLazy();        

        Container.BindInterfacesAndSelfTo<CharactersMerge>().AsSingle().Lazy();

        BindApplicationLayer();

        BindRendering();
        BindPageManager();
        BindLocalizationManager();
        
        BindGameLayer();
        BindFactories();
        BindDeepLinkManager();

        DeclareSignals();
        
        Container.BindInterfacesAndSelfTo<PopupsInitializer>().AsSingle().NonLazy();
        UnityEngine.Debug.LogWarning("ProjectContextInstaller Complete");
    }

    private void BindDeepLinkManager()
    {
        Container.BindFactory<string, DeepLinkManager, DeepLinkManager.Factory>().FromFactory<PrefabResourceFactory<DeepLinkManager>>();
    }

    private void BindPageManager()
    {
        Container.BindInterfacesAndSelfTo<PageManager>().AsSingle().NonLazy();

        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<InititalizeUISignal>();
    }

    private void DeclareSignals()
    {
        Container.DeclareSignal<NotifyingPopupSignal>();
        Container.DeclareSignal<StateSelectPopupSignal>();
        Container.DeclareSignal<DisintegratePopupSignal>();
        Container.DeclareSignal<ToShopMovePopupSignal>();
        Container.DeclareSignal<UpgradeCharacterPopupSignal>();

        Container.DeclareSignal<OpenUIViewPageSignal>();
        Container.DeclareSignal<OpenUIPopUpSignal>();
        Container.DeclareSignal<PortalPopupSignal>();
        Container.DeclareSignal<CharacterMergePopupSignal>();
        Container.DeclareSignal<SortSignal>();
        Container.DeclareSignal<GridWidgetSignal>();

        Container.DeclareSignal<AboutPositionPopupSignal>();
        Container.DeclareSignal<AboutRarityPopupSignal>();
        Container.DeclareSignal<AboutElementsPopupSignal>();

        Container.DeclareSignal<LocationWidgetSignal>();
        Container.DeclareSignal<PlanetWidgetSignal>();
        Container.DeclareSignal<MatchStateSignal>();

        Container.DeclareSignal<DragableUIVIewSignal>();
    }

    private void BindLocalizationManager()
    {
        Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle().NonLazy();
    }    

    private void BindRendering()
    {
        Container.Bind<Graphics>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RenderLayerBackground>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RenderLayer3DPlanet>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RenderLayerSpine>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RenderLayerMainUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RenderLayerVFXOverlay>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RenderManager>().AsSingle().NonLazy();
    }

    private void BindApplicationLayer()
    {
        Container.BindInterfacesAndSelfTo<FirebaseAppFacade>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ResourcesManager>().AsSingle().NonLazy();
    }

    private void BindGameLayer()
    {
        Container.BindInterfacesAndSelfTo<CharactersManager>().AsSingle().NonLazy();
        Container.DeclareSignalWithInterfaces<GameFlowMessage>();
        Container.BindInterfacesAndSelfTo<GameFlowMessageHandler>().AsSingle().NonLazy();
        Container.DeclareSignalWithInterfaces<MapsWidgetMessage>();

        Container.BindMemoryPool<Pin, Pin.Pool>()
            .WithInitialSize(35)
            .FromComponentInNewPrefabResource(CommonStrings.PrefabPin)
            .UnderTransformGroup("Pins");
    }

    private void BindFactories()
    {
        Container.BindFactory<string, PortalSummonPopup, PortalSummonPopup.Factory>().FromFactory<PortalSummonPopup.Factory>();
        Container.BindFactory<string, Character, TeamType, int, UnityEngine.Transform, MatchResultCharacterView, MatchResultCharacterView.Factory>().FromFactory<MatchResultCharacterView.Factory>();
        Container.BindFactory<string, MinimalCharacterView, MinimalCharacterView.Factory>().FromFactory<PrefabResourceFactory<MinimalCharacterView>>();
        Container.BindFactory<string, CartForUpdate, CartForUpdate.Factory>().FromFactory<PrefabResourceFactory<CartForUpdate>>();
        Container.BindFactory<string, CartSlotForGrid, CartSlotForGrid.Factory>().FromFactory<PrefabResourceFactory<CartSlotForGrid>>();

        Container.BindFactory<string, NotifyingPopup, PopupBase<NotifyingPopup>.Factory>().FromFactory<PopupBase<NotifyingPopup>.Factory>();
        Container.BindFactory<string, DisintegratePopup, PopupBase<DisintegratePopup>.Factory>().FromFactory<PopupBase<DisintegratePopup>.Factory>();
        Container.BindFactory<string, StateSelectPopup, PopupBase<StateSelectPopup>.Factory>().FromFactory<PopupBase<StateSelectPopup>.Factory>();
        Container.BindFactory<string, ToShopMovePopup, PopupBase<ToShopMovePopup>.Factory>().FromFactory<PopupBase<ToShopMovePopup>.Factory>();
        Container.BindFactory<string, UpgradeCharacterPopup, PopupBase<UpgradeCharacterPopup>.Factory>().FromFactory<PopupBase<UpgradeCharacterPopup>.Factory>();

        Container.BindFactory<string, AboutPositionPopup, PopupBase<AboutPositionPopup>.Factory>().FromFactory<PopupBase<AboutPositionPopup>.Factory>();
        Container.BindFactory<string, AboutRarityPopup, PopupBase<AboutRarityPopup>.Factory>().FromFactory<PopupBase<AboutRarityPopup>.Factory>();
        Container.BindFactory<string, AboutElementsPopup, PopupBase<AboutElementsPopup>.Factory>().FromFactory<PopupBase<AboutElementsPopup>.Factory>();

        Container.BindFactory<Character, TeamType, UnityEngine.Transform, DetailCharacterView, DetailCharacterView.Factory>().FromMonoPoolableMemoryPool(
            e => e.WithInitialSize(12).FromComponentInNewPrefabResource(CommonStrings.PrefabDetailCharacterView).UnderTransformGroup("DetailCharacters")).Lazy();
        Container.BindFactory<CharacterPositionPlaceholder, CharacterPositionPlaceholder.Factory>().FromMonoPoolableMemoryPool(
            e => e.WithInitialSize(12).FromComponentInNewPrefabResource(CommonStrings.PrefabCharacterPositionPlaceholder).UnderTransformGroup("CharacterPositionPlaceholder")).Lazy();
    }
}
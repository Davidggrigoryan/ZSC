#if DOOZY_PRESENT
﻿using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.GameLayer.UI;
using ZombieSoccer.Rendering;

namespace ZombieSoccer.UI.ViewPortalM
{
    public class PortalSummonPopup : MonoBehaviour
    {        

        [Inject]
        SignalBus signalBus;

        [Inject]
        DetailCharacterView.Factory detailCharacterViewFactory;

        [Inject]
        RenderManager renderManager;

        [SerializeField]
        UIPopup self;

        [SerializeField]
        Transform detailCharacterContainer;

        [SerializeField]
        GameObject darkMask;        

        [SerializeField]
        GameObject takeButton;

        [SerializeField]
        GameObject characterName;

        public void Awake()
        {
            signalBus.Subscribe<PortalPopupSignal>(Show);
            renderManager.ResolveLayer<RenderLayerVFXOverlay>().SetTransformIntoContainer(this.transform);
            self.Hide(false);
            darkMask.SetActive(false);
        }

        public void Show(PortalPopupSignal payload)
        {
            darkMask.SetActive(true);
            
            var character = payload.character;

            var detailCharacterView = detailCharacterViewFactory.Create(character, TeamType.Ally, detailCharacterContainer.transform);

            takeButton.GetComponent<Button>().onClick.RemoveAllListeners();            
            takeButton.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                self.Hide();
                darkMask.SetActive(false);
            }));

            self.Show();
            
            characterName.GetComponent<TMPro.TextMeshProUGUI>().text = character.characterName;
        }
        
        public class Factory : PlaceholderFactory<string, PortalSummonPopup> {
            readonly DiContainer _container;

            public Factory(DiContainer container)
            {
                _container = container;
            }

            public override PortalSummonPopup Create(string prefabPath)
            {
                return _container.InstantiatePrefabResourceForComponent<PortalSummonPopup>(prefabPath);
            }
        }
    }
}

#endif

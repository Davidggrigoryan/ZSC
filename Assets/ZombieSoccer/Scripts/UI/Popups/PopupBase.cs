using Doozy.Engine.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ZombieSoccer.UI.Popups
{
    public abstract class PopupBase <TPopup>: MonoBehaviour
    {
        [Inject]
        SignalBus signalBus;

        protected UIPopup popup;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            Enable();
            popup ??= this.GetComponent<UIPopup>();
        }

        private void OnDisable()
        {
            UnsubscribeAction();
            Disable();
        }

        protected virtual void Initialize() { }
        protected virtual void Enable() { }
        protected virtual void Disable() { }
        private Action UnsubscribeAction;

        protected void SetupSignalReceiver<TSignal>(Action<TSignal> signalReceiverAction) where TSignal : new()
        {
            signalBus?.Subscribe<TSignal>((payload) => { signalReceiverAction.Invoke(payload); });            
            UnsubscribeAction = () => { signalReceiverAction.Invoke(new TSignal()); };
        }

        public class Factory : PlaceholderFactory<string, TPopup>
        {
            readonly DiContainer _container;

            public Factory(DiContainer container)
            {
                _container = container;
            }

            public override TPopup Create(string pathToPrefab)
            {
                return _container.InstantiatePrefabResourceForComponent<TPopup>(pathToPrefab);
            }
        }
    }
}

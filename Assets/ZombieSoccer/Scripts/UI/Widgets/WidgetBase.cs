using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ZombieSoccer.UI.Widget
{
    public abstract class WidgetBase : MonoBehaviour
    {
        [FoldoutGroup("General"), PropertyOrder(-1), ShowInInspector, ReadOnly]
        public bool IsInitialized { get; protected set; } = false;

        [Inject]
        private readonly SignalBus signalBus;

        protected CompositeDisposable disposables = new CompositeDisposable();

        [SerializeField] protected GameObject root;

        private void Awake()
        {
            signalBus.Subscribe<InititalizeUISignal>(Inititalize);
        }

        protected virtual void Inititalize()
        {
            IsInitialized = true;
            Debug.Log($"[Widget.Inititalize] Name: {gameObject.name}");
        }

        [Button]
        public virtual void Enable()
        {
            if (!IsInitialized)
                return;

            Debug.Log($"[Widget.Enable] Name: {gameObject.name}");
        }

        [Button]
        public virtual void Disable()
        {
            if (!IsInitialized)
                return;

            Debug.Log($"[Widget.Enable] Name: {gameObject.name}");
            disposables.Clear();
        }
    }
}

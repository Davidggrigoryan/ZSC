using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using ZombieSoccer.UI.Widget;
using Cysharp.Threading.Tasks;

namespace ZombieSoccer.UI.Presenters
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PageSignalHandler))]
    public abstract class BasePagePresenter : MonoBehaviour
    {
        #region GENERAL
        [FoldoutGroup("General"), PropertyOrder(-1), ShowInInspector, ReadOnly]
        public bool IsInitialized { get; protected set; } = false;

        [FoldoutGroup("General"), PropertyOrder(-1), ShowInInspector]
        public Transform Content { get; private set; }

        [FoldoutGroup("General"), PropertyOrder(1), ShowInInspector]
        public List<WidgetBase> WidgetsDeps { get; protected set; } = new List<WidgetBase>();

        protected CompositeDisposable disposables = new CompositeDisposable();

        protected PageSignalHandler pageSignalHandler;

        [Inject]
        private readonly SignalBus signalBus;
        #endregion

        #region MONOBEH
        private void Awake()
        {
            signalBus.Subscribe<InititalizeUISignal>(Inititalize);
            WidgetsDeps = GetComponentsInChildren<WidgetBase>().ToList();
            pageSignalHandler = GetComponent<PageSignalHandler>();

//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.playmodeStateChanged += ModeChanged;
//#endif
        }

        [Button]
        protected virtual void OnEnable()
        {
            if (!IsInitialized)
                return;

            WidgetsDeps.ForEach(widget => widget.Enable());
            Enable();

            //For example
            //Observable.EveryUpdate().Subscribe(x => Debug.Log(x)).AddTo(disposables);
        }

        protected virtual void Enable() { }

        [Button]
        private void OnDisable()
        {
            if (!IsInitialized)
                return;

            WidgetsDeps.ForEach(widget => widget.Disable());
            Disable();            
            disposables.Clear();
        }

        protected virtual void Disable() { }
        #endregion


        #region BASE CLASS
        protected virtual void Inititalize() => IsInitialized = true;

        public UniTask task { get; protected set; }

        public virtual async UniTask PerformTaskAsync()
        {
            await UniTask.Yield();
        }

        #endregion
    }
}

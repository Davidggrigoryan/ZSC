#if DOOZY_PRESENT
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ZombieSoccer.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIView))]
    public class PageSignalHandler : MonoBehaviour
    {
        private string pageName;
        public bool instant;

        [Inject]
        private readonly PageManager pageManager;

        [Inject]
        private readonly SignalBus signalBus;

        public System.Object[] args { get; set; }

        private UIView view;

        public void Setup(string pageName)
        {
            Debug.LogError($"[PageSignalHandler.Setup] PageName: {pageName}");
            gameObject.name = this.pageName = pageName;
            signalBus.Subscribe<OpenUIViewPageSignal>(SignalReceiver);
            view = GetComponent<UIView>();
        }

        private void SignalReceiver(OpenUIViewPageSignal payload)
        {            
            //if ( payload.type == TargetType.VIEW )
            //{
            if(pageName == payload.targetPage)
            {
                args = payload.args;
                view.Show(instant);
                Debug.LogWarning(
                    $"[PageSignalHandler] SHOW: {payload.targetPage}\n" +
                    $"GO: {gameObject.name}\n" +
                    $"Instant: {instant}");
            }
            else
                view.Hide(instant);
        }


        [Button]
        public void Fire() => pageManager.Fire(this.pageName);

        [Button]
        public void FireMessage(MsgTypeEnum msgType, string msg) => pageManager.FireMessage(msgType, msg);

        public void Fire(string _pageName) => pageManager.Fire(_pageName);

        public void Dispose() => signalBus.Unsubscribe<OpenUIViewPageSignal>(SignalReceiver);

    }
}

#endif

using Doozy.Engine.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer.UI.Popups
{
    public sealed class AboutPositionPopupSignal
    {
        public Action<Transform> begin;
        public Action end;
    }

    [DisallowMultipleComponent]
    public sealed class AboutPositionPopup : PopupBase<AboutPositionPopup>
    {

        [SerializeField] Button hideButton;
        
        Action _endAction;

        protected override void Initialize()
        {
            base.Initialize();
            hideButton.onClick.AddListener(() =>
            {
                this.GetComponent<UIPopup>().Hide();
                _endAction?.Invoke();
            });
        }

        protected override void Enable()
        {
            base.Enable();
            SetupSignalReceiver<AboutPositionPopupSignal>(AboutPositionPopupSignalReceiver);
        }

        
        private void AboutPositionPopupSignalReceiver(AboutPositionPopupSignal signal)
        {
            base.popup.Show();
            signal.begin.Invoke(this.transform);
            _endAction = signal.end;
        }

    }
}

using Doozy.Engine.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer.UI.Popups
{
    public sealed class AboutElementsPopupSignal
    {
        public Action<Transform> begin;
        public Action end;
    }

    [DisallowMultipleComponent]
    public sealed class AboutElementsPopup : PopupBase<AboutElementsPopup>
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
            SetupSignalReceiver<AboutElementsPopupSignal>(AboutElementsPopupSignalReceiver);
        }

        private void AboutElementsPopupSignalReceiver(AboutElementsPopupSignal signal)
        {
            base.popup.Show();
            signal.begin.Invoke(this.transform);
            _endAction = signal.end;
        }
    }
}

using Doozy.Engine.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer.UI.Popups
{
    public sealed class AboutRarityPopupSignal
    {
        public Action<Transform> begin;
        public Action end;
    }

    [DisallowMultipleComponent]
    public sealed class AboutRarityPopup : PopupBase<AboutRarityPopup>
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
            SetupSignalReceiver<AboutRarityPopupSignal>(AboutRarityPopupSignalReceiver);
        }

        private void AboutRarityPopupSignalReceiver(AboutRarityPopupSignal signal)
        {
            base.popup.Show();
            signal.begin.Invoke(this.transform);
            _endAction = signal.end;
        }
    }
}

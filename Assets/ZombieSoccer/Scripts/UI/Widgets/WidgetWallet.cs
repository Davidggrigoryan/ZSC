using UnityEngine;
using TMPro;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;

namespace ZombieSoccer.UI.Widget
{
    public class WidgetWallet : WidgetBase
    {
        [Inject]
        private UserModel user { get; }

        [SerializeField]
        private TextMeshProUGUI m_elixir;

        [SerializeField]
        private TextMeshProUGUI m_dust;

        [SerializeField]
        private TextMeshProUGUI m_crystal;

        public override void Enable()
        {
            base.Enable();

            user.Wallet.OnDataChangedAction += UpdateData;
            UpdateData();
        }

        public override void Disable()
        {
            base.Disable();

            user.Wallet.OnDataChangedAction -= UpdateData;
        }

        private void UpdateData()
        {
            m_elixir.text       = user.Wallet.newData.Elixir.KiloFormat();
            m_dust.text         = user.Wallet.newData.Dust.KiloFormat();
            m_crystal.text      = user.Wallet.newData.Crystal.KiloFormat();
        }
    }
}
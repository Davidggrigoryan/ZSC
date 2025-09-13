using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using ZombieSoccer.ApplicationLayer.Data;

namespace ZombieSoccer.UI.Widget
{
    public class WidgetResources : WidgetBase
    {
        [FoldoutGroup("Elixir")]
        [SerializeField] Transform elixirTransform;
        [FoldoutGroup("Elixir")]
        [SerializeField] TextMeshProUGUI costElixir, inStockElixir;

        [FoldoutGroup("Dust")]
        [SerializeField] Transform dustTransform;
        [FoldoutGroup("Dust")]
        [SerializeField] TextMeshProUGUI costDust, inStockDust;

        [FoldoutGroup("Crystal")]
        [SerializeField] Transform crystalTransform;
        [FoldoutGroup("Crystal")]
        [SerializeField] TextMeshProUGUI costCrystal, inStockCrystal;

        public override void Enable()
        {
            base.Enable();
            SetToZero();
        }

        public void UpdateValues(Wallet cost, Wallet inStock)
        {
            UpdateValue(elixirTransform, costElixir, inStockElixir, cost.Elixir, inStock.Elixir);
            UpdateValue(dustTransform, costDust, inStockDust, cost.Dust, inStock.Dust);
            UpdateValue(crystalTransform, costCrystal, inStockCrystal, cost.Crystal, inStock.Crystal);
        }

        private void SetToZero()
        {
            UpdateValues(new Wallet() { Crystal = 0, Dust = 0, Elixir = 0 },
                         new Wallet() { Crystal = 0, Dust = 0, Elixir = 0 });
        }

        private void UpdateValue(Transform container, TextMeshProUGUI costText, TextMeshProUGUI inStockText, int cost, int inStock)
        {
            if (cost == 0)
            {
                container.gameObject.SetActive(false);
                return;
            }
            if(!container.gameObject.activeSelf)
                container.gameObject.SetActive(true);
            costText.text = cost.KiloFormatAccurate();
            inStockText.text = inStock.KiloFormat();
            if (cost > inStock )
                inStockText.color = new Color(1F, 0.3F, 0.3F, 1F);
            else inStockText.color = Color.white;

        }
    }
}

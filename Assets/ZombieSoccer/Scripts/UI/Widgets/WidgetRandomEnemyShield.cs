using UnityEngine;

namespace ZombieSoccer.UI.Widget
{
    public class WidgetRandomEnemyShield : WidgetShield
    {
        public override void Enable()
        {
            Preview(
                Random.Range(0, shieldPallete.shieldsSprites.Length),
                Random.Range(0, shieldPallete.detailsSprites.Length),
                Random.Range(0, shieldPallete.tapesSprites.Length),
                Random.Range(0, shieldPallete.ballsSprites.Length),
                null);
        }
    }
}

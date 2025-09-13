using TMPro;
using UnityEngine.UI;

namespace ZombieSoccer.Extensions
{
    public static class ButtonExtensions
    {
        public static Button ChangeAlpha( this Button b, float alpha)
        {
            var childs = b.GetComponentsInChildren<Image>();
            
            foreach(var child in childs)
            {
                var c = child.color;
                c.a = alpha;
                child.color = c;                
            }

            var childs1 = b.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var child in childs1)
            {
                var c = child.color;
                c.a = alpha;
                child.color = c;
            }

            return b;
        }
    }
}

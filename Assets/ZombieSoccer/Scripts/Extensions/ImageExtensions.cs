using TMPro;
using UnityEngine.UI;

namespace ZombieSoccer.Extensions
{
    public static class ImageExtensions
    {
        public static Image ChangeAlpha(this Image img, float alpha)
        {
            var c = img.color;
            c.a = alpha;
            img.color = c;
            return img;
        }
    }
}

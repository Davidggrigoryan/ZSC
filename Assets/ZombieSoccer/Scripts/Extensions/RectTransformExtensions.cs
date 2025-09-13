using UnityEngine;

namespace ZombieSoccer.Utils.RectTransformExtensions
{
    public static class RectTransformExtensions
    {
        public static bool Overlaps(this RectTransform a, RectTransform b)
        {
            return a.Rect().Overlaps(b.Rect());
        }

        public static Rect Rect(this RectTransform rectTransform)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            Vector3 position = rectTransform.position;
            return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
        }
    }
}

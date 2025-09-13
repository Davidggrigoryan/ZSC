using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer
{
    public class GridLayoutGroupHelper : MonoBehaviour
    {
        public float width;
        public float height;

        public Vector2Int count;

        [Button]
        void Start()
        {
            width = this.gameObject.GetComponent<RectTransform>().rect.width;
            height = this.gameObject.GetComponent<RectTransform>().rect.height;

            Vector2 newSize = new Vector2(width / (float)count.x, height / (float)count.y);
            this.gameObject.GetComponent<GridLayoutGroup>().cellSize = newSize;
        }
    }
}

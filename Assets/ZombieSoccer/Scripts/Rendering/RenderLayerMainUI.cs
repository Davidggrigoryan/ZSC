using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ZombieSoccer.Rendering
{
    [InfoBox("General UI")]
    public class RenderLayerMainUI : RenderLayerBase, IInitializable
    {
        private RectTransform m_rectContainer;

        [Inject]
        public void Initialize()
        {
            Debug.LogWarning("[RenderLayer.MainUI] Setup...");
            renderManager.SetRenderLayer(typeof(RenderLayerMainUI), this);
            m_rectContainer = m_container.GetComponent<RectTransform>();
        }

        //public override void SetTransformIntoContainer(Transform targetTransform)
        //{
        //    m_childs.Add(targetTransform);
        //    var rectTransform = targetTransform.GetComponent<RectTransform>();

        //    rectTransform.transform.SetParent(m_rectContainer, false);

        //    rectTransform.anchoredPosition = m_rectContainer.position;
        //    rectTransform.anchorMin = new Vector2(1, 0);
        //    rectTransform.anchorMax = new Vector2(0, 1);
        //    rectTransform.pivot = new Vector2(0.5f, 0.5f);
        //    rectTransform.sizeDelta = m_rectContainer.rect.size;
        //}
    }
}
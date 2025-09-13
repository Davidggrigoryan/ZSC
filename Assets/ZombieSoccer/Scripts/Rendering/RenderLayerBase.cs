using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZombieSoccer.Rendering
{
    public abstract class RenderLayerBase : MonoBehaviour
    {
        [Inject]
        protected RenderManager renderManager;

        public Camera Camera => GetComponentInChildren<Camera>();

        /// <summary>
        /// Root parenting transform
        /// </summary>
        [SerializeField]
        protected Transform m_container;

        public Transform Container => m_container;

        public bool m_ignoreMainCameraStack = false;

        /// <summary>
        /// In general its contains pages
        /// </summary>
        [SerializeField]
        public List<Transform> m_childs = new List<Transform>();

        public virtual void SetTransformIntoContainer(Transform targetTransform)
        {
            m_childs.Add(targetTransform);
            targetTransform.SetParent(m_container, false);
        }
    }
}
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace ZombieSoccer.Rendering
{
    [InfoBox("3d planet")]
    public class RenderLayer3DPlanet : RenderLayerBase, IInitializable
    {
        private Camera m_targetCamera;
        private UniversalAdditionalCameraData m_universalAdditionalCameraData;

        public bool m_usePostProcess = true;

        [Inject]
        public void Initialize()
        {
            Debug.LogWarning("[RenderLayer.3dPlanet] Setup...");
            renderManager.SetRenderLayer(typeof(RenderLayer3DPlanet), this);

            m_targetCamera = GetComponentInChildren<Camera>();
            m_universalAdditionalCameraData = GetComponentInChildren<UniversalAdditionalCameraData>();
            m_universalAdditionalCameraData.renderPostProcessing = m_usePostProcess;
        }
    }
}
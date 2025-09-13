using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ZombieSoccer.Rendering
{
    [InfoBox("Overlay layer: VFX")]
    public class RenderLayerVFXOverlay : RenderLayerBase, IInitializable
    {
        [Inject]
        public void Initialize()
        {
            Debug.LogWarning("[RenderLayer.VFXOverlay] Setup...");

            renderManager.SetRenderLayer(typeof(RenderLayerVFXOverlay), this);
        }
    }
}
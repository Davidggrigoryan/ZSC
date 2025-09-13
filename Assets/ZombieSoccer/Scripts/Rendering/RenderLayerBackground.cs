using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ZombieSoccer.Rendering
{
    [InfoBox("Animated backgrounds")]
    public class RenderLayerBackground : RenderLayerBase, IInitializable
    {
        [Inject]
        public void Initialize()
        {
            Debug.LogWarning("[RenderLayer.Background] Setup...");
            renderManager.SetRenderLayer(typeof(RenderLayerBackground), this);
        }
    }
}
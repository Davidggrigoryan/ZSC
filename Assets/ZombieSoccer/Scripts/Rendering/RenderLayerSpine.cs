using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ZombieSoccer.Rendering
{
    [InfoBox("Spine")]
    public class RenderLayerSpine : RenderLayerBase, IInitializable
    {
        [Inject]
        public void Initialize()
        {
            Debug.LogWarning("[RenderLayer.Spine] Setup...");
            renderManager.SetRenderLayer(typeof(RenderLayerSpine), this);
        }
    }
}
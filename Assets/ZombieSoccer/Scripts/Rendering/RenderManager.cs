using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System;
using UnityEngine.Rendering.Universal;

namespace ZombieSoccer.Rendering
{
    //[System.Serializable]
    public class RenderManager : IInitializable
    {
        [Inject]
        DiContainer container;
        public static Camera MainCamera => Camera.main;
        public static UniversalAdditionalCameraData URPSettingsMainCamera 
            => MainCamera.transform.GetComponent<UniversalAdditionalCameraData>();

        [SerializeField]
        private List<RenderLayerBase> m_rendersList;

        public Dictionary<Type, RenderLayerBase> Layers { get; private set; } = new Dictionary<Type, RenderLayerBase>();

        public void Initialize()
        {
            Debug.LogWarning("[RenderManager] Initialize");
            //URPSettingsMainCamera.cameraStack.AddRange(Camera.main.transform.GetComponentsInChildren<Camera>());
        }

        public void SetRenderLayer(Type layerType, RenderLayerBase renderLayer)
        {
            Layers.Add(layerType, renderLayer);

            if(!renderLayer.m_ignoreMainCameraStack)
                URPSettingsMainCamera.cameraStack.Add(renderLayer.Camera);
        }

        public RenderLayerBase ResolveLayer<T>()
        {
            return Layers[typeof(T)];
        }

        //public void SetTransformIntoContainer<T>(Transform targetTransform)
        //{
        //    var targetRenderLayer = ResolveLayer(typeof(T));
        //    targetRenderLayer.SetTransformIntoContainer(targetTransform);
        //}

        //public RenderLayerBase ResolveLayer(Type type)
        //{
        //    foreach (var r in m_rendersList)
        //        if (r.GetType() == type)
        //            return r;

        //    return null;
        //}
    }
}
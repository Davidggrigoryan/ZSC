using InputObservable;
using System;
using UnityEngine;
using Zenject;
using ZombieSoccer.Rendering;
using ZombieSoccer.UI.Widget;

namespace ZombieSoccer.UI
{
    public sealed class PlanetWidgetSignal
    {
        public bool Show { get; set; }
        public Action action { private get; set; }
    }

    public sealed class PlanetWidget : WidgetBase
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private RenderManager _renderManager;

        [SerializeField] private GameObject target;
        
        private InputObservableContext _context;

        protected override void Inititalize()
        {
            base.Inititalize();
            _signalBus.Subscribe<PlanetWidgetSignal>(PlanetWidgetSignalReceiver);
            _context = this.DefaultInputContext();
        }

        public override void Enable()
        {
            base.Enable();
            _signalBus.Fire(new LocationWidgetSignal()
            {
                Show = false
            });

            var renderLayer3DPlanet = _renderManager.ResolveLayer<RenderLayer3DPlanet>() as RenderLayer3DPlanet;
            renderLayer3DPlanet.SetTransformIntoContainer(target.transform);
            var mainCamera = _renderManager.ResolveLayer<RenderLayer3DPlanet>().Camera;

            new PlanetRotateOnTouch().BuildPipeline(_context)
                .SetTriggerToReturn()
                .SetScaleTrigger()
                .SetCamera(mainCamera)
                .SetTargetObject(target)
                .SetCompositeDisposable(disposables)
                .Subscribe();
        }

        private void PlanetWidgetSignalReceiver(PlanetWidgetSignal message)
        {
            switch (message.Show)
            {
                case true:
                    base.root.SetActive(true);
                    break;
                case false:
                    base.root.SetActive(false);
                    break;
            }
        }
    }
}

using UnityEngine;
using Zenject;
using ZombieSoccer.UI.Presenters;

namespace ZombieSoccer.UI.Views
{
    //[RequireComponent(typeof(PageSignalHandler))]
    public class MainMenuPresenter : BasePagePresenter
    {
        //[Inject]
        //private RenderManager renderManager;

        //[SerializeField]
        //private Transform map3d, map2d;

        //[SerializeField]
        //private PlanetMapCameraController mapController;

        //private RenderLayer3DPlanet renderLayer3DPlanet;

        //public TMPro.TextMeshProUGUI textMeshProUGUI;

        ////protected override void Start()
        ////{           
        ////    mapController = GetComponentInChildren<PlanetMapCameraController>();
        ////    renderLayer3DPlanet = renderManager.ResolveLayer<RenderLayer3DPlanet>() as RenderLayer3DPlanet;
        ////    renderLayer3DPlanet.SetTransformIntoContainer(map3d);
        ////    mapController.mainCamera = renderLayer3DPlanet.Camera;
        ////    mapController.ResetMap();
        ////}

        //protected override void Inititalize()
        //{
        //    base.Inititalize();
        //}

        //protected override void Enable()
        //{
        //    ////Debug.LogError("MainMenuPresenter is enable");
        //    //// Get IInputObservableContext
        //    //var context = this.DefaultInputContext();

        //    //// Get IInputObservable with id=0, left button for Editor, fingerId=0 for Android/iOS
        //    //IInputObservable touch0 = context.GetObservable(0);

        //    //// Get IInputObservable with id=1, right button for Editor, fingerId=1 for Android/iOS
        //    //IInputObservable touch1 = context.GetObservable(1);

        //    ////RectangleObservable.From(touch0, touch1)
        //    ////    .PinchSequence()
        //    ////    .RepeatUntilDestroy(this)
        //    ////    .Subscribe(diff =>  // diff is Vector2
        //    ////    {

        //    ////        textMeshProUGUI.text = $"${t} - {diff.ToString()}";
        //    ////    })
        //    ////    .AddTo(disposables);
        //    //ReactiveProperty<Vector2> startsize = new ReactiveProperty<Vector2>(Vector2.zero);

        //    //var startTouchesMerge = Observable.Merge(touch0.OnBegin, touch1.OnBegin);
        //    //var stream = Observable.CombineLatest(touch0.Any(), touch1.Any())
        //    //    .TakeUntil(startTouchesMerge)
        //    //    .Select(es =>
        //    //    {
        //    //        var x = Mathf.Min(es[0].position.x, es[1].position.x);
        //    //        var y = Mathf.Min(es[0].position.y, es[1].position.y);
        //    //        return new Rect(x, y,
        //    //            Mathf.Abs(es[0].position.x - es[1].position.x),
        //    //            Mathf.Abs(es[0].position.y - es[1].position.y));
        //    //    })
        //    //    .DistinctUntilChanged();



        //    //RectangleObservable.From(touch0, touch1)
        //    //    .PinchSequence()
        //    //    .RepeatUntilDestroy(this)
        //    //    .Subscribe(diff =>  // diff is Vector2
        //    //    {

        //    //        textMeshProUGUI.text = $"{diff.ToString()}";
        //    //    })
        //    //    .AddTo(disposables);
        //}

        //protected override void Disable()
        //{

        //}
    }
}

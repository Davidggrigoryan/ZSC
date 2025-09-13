using Cysharp.Threading.Tasks;
using Doozy.Engine.UI;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Video;
using ZombieSoccer.UI.Presenters;

namespace ZombieSoccer.UI.Views
{
    [RequireComponent(typeof(UIView))]
    [RequireComponent(typeof(PageSignalHandler))]
    
    public class ViewIntroTrailer : BasePagePresenter
    {
        [SerializeField]
        private Button skipButton;

        [SerializeField]
        private PlayableDirector director;

        [SerializeField]
        private VideoPlayer videoPlayer;

        protected override void Enable()
        {
            base.Enable();

            videoPlayer.targetCamera = Camera.main;

            skipButton
                .OnClickAsObservable()
                .Subscribe(_ => director.Stop())
                .AddTo(disposables);

            task = director.PlayAsync();
        }
    }
}

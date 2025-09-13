using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using ZombieSoccer.Database;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.UI.Presenters;
using Cysharp.Threading.Tasks;

namespace ZombieSoccer.UI.Views
{
    public class ViewDataLoader : BasePagePresenter
    {
        [SerializeField]
        private string nextPage;

        [SerializeField]
        private Slider progressBar;

        [Inject]
        FirebaseAppFacade firebaseAppFacade;

        [Inject]
        CharactersManager characterManager;

        protected override void Enable()
        {
            base.Enable();

            progressBar.value = 0f;
            // do something, for example upload character's images from CDN
            task = UniTask
                .WaitWhile(() => !firebaseAppFacade.IsComplete && !characterManager.IsComplete)
                .ContinueWith(() =>
                {
                    progressBar.value = 1f;
                    Debug.LogWarning("[DataLoader.UpdateData] Complete");
                });
        }
    }
}

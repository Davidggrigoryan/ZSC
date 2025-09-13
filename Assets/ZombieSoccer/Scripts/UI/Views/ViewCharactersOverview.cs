using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.GameLayer.UI;
using ZombieSoccer.Models.DeckM;
using ZombieSoccer.Extensions;
using ZombieSoccer.UI.Presenters;
using ZombieSoccer.Utils.LocalStorage;
using System.Collections.Generic;
using Doozy.Engine.Extensions;
using ZombieSoccer.Utils.RectTransformExtensions;
using UniRx;
using ZombieSoccer.ReactiveInput;
using InputObservable;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using ZombieSoccer.Utils.ListOfViewExtensions;

namespace ZombieSoccer.UI.Views
{
    public class ViewCharactersOverview : BasePagePresenter
    {
        [Inject]
        DetailCharacterView.Factory detailCharacterViewFactory;

        [Inject]
        PageManager pageManager;

        [Inject]
        DeckModel deckModel;        

        [Inject]
        SignalBus signalBus;

        [SerializeField]
        private GridLayoutGroup gridLayoutGroup;

        [SerializeField] Transform viewport;

        [SerializeField] ScrollRect scrollRect;

        public float characterViewSizeRation = 0.85f;

        private List<DetailCharacterView> detailCharacterViews = new List<DetailCharacterView>();

        protected async override void Enable()
        {
            signalBus.Subscribe<SortSignal>(SortSignalReceiver);

            deckModel.SortWithSeparateNew<DefaultDeckSort>();
            
            scrollRect.OnValueChangedAsObservable().Subscribe(_ => ListOfViewsExtension.DeactivateNonIntersectViews<DetailCharacterView>(viewport) ).AddTo(disposables);
            await UniTask.Yield();

            SetToFirstPosition();
            ListOfViewsExtension.DeactivateNonIntersectViews<DetailCharacterView>(viewport);
        }

        protected override void Disable()
        {
            signalBus.Unsubscribe<SortSignal>(SortSignalReceiver);
            detailCharacterViews.ForEach(e => e.Dispose());
        }

        private void SetToFirstPosition()
        {
            var y = gridLayoutGroup.GetComponent<RectTransform>().sizeDelta.y;
            gridLayoutGroup.GetComponent<RectTransform>().localPosition = new Vector2(0, -y / 2);
        }        

        private void SortSignalReceiver(SortSignal sortSignal)
        {
            gridLayoutGroup.Prepare();
            detailCharacterViews = gridLayoutGroup.SpawnDetailCharacterViews(sortSignal.characters, detailCharacterViewFactory, OnClickViewEventHandler);
        }

        public void OnClickViewEventHandler(Character character)
        {
            Debug.LogError($"Select character: {character.archetypeId}");
            pageManager.Fire("UIView_CharacterOverviewDetail", (System.Object)character);
        }
    }
}
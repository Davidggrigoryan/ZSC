using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.EventSystems;
using Zenject;
using InputObservable;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using ZombieSoccer.GameLayer.UI;

namespace ZombieSoccer.ReactiveInput
{
    public class ObservableInputContextMessage
    {


        public class OnBegin : ObservableInputMessage
        {

        }

        public class OnAction : ObservableInputMessage
        {

        }

        public class OnComplete : ObservableInputMessage
        {

        }

        public class OnSubscribe : ObservableInputMessage
        {

        }
    }

    public class ObservableInputContextService : IInitializable
    {
        [Inject]
        private SignalBus signalBus;

        private InputObservableContext _inputObservableContext;
        private EventSystem _eventSystem;
        private CompositeDisposable disposables = new CompositeDisposable();

        public ObservableInputContextService()
        {

        }

        public void Initialize()
        {

        }

        private InputObservableContext GetDefaultInputContext(MonoBehaviour behaviour, EventSystem eventSystem)
        {
            _inputObservableContext ??= behaviour.DefaultInputContext(eventSystem);

            return _inputObservableContext;
        }

        private InputObservableContext GetDefaultInputContext(MonoBehaviour behaviour)
        {
            _inputObservableContext ??= behaviour.DefaultInputContext();

            return _inputObservableContext;
        }

        public virtual IInputObservable GetDefaultInputContextObservable(MonoBehaviour behaviour,
            EventSystem eventSystem, int observableId = 0)
        {
            return GetDefaultInputContext(behaviour, eventSystem).GetObservable(observableId);
        }

        public virtual IInputObservable GetDefaultInputContextObservable(MonoBehaviour behaviour, int observableId = 0)
        {
            return GetDefaultInputContext(behaviour).GetObservable(observableId);
        }

        public virtual void ClearAll()
        {
            if (disposables.Count > 0)
            {
                Debug.Log("Observables cleared");
                disposables.Clear();
            }
        }

        public virtual void DragAndDrop(IInputObservable io,
            Action<InputEvent> onBegin,
            Action<InputEvent> onMove,
            Action<InputEvent> onEnd,
            Action outerOnCompleted,
            Action onUnsubscribe,
            MonoBehaviour behaviour,
            Func<InputEvent, bool> takeWhileFunc)
        {

            io.OnBegin.TakeUntilDestroy(behaviour).TakeWhile(takeWhileFunc).Subscribe(x =>
            {
                if (behaviour != null)
                {
                    onBegin.Invoke(x);
                    io.OnMove.TakeUntil(io.OnEnd).Subscribe(onMove).AddTo(disposables);
                    io.OnEnd.First().Finally(() =>
                    {
                        outerOnCompleted.Invoke();
                    }).Subscribe(onEnd).AddTo(disposables);
                }
                
            }, () =>
            {
                onUnsubscribe.Invoke();
                ClearAll();
            });
        }

        public virtual void DragAndDrop(IInputObservable io,
            ObservableInputObjectWrapper onBegin,
            ObservableInputObjectWrapper onMove,
            ObservableInputObjectWrapper onEnd,
            Action outerOnCompleted,
            Action onUnsubscribe,
            MonoBehaviour behaviour,
            Func<InputEvent, bool> takeWhileFunc)
        {
            io.OnBegin.TakeUntilDestroy(behaviour).TakeWhile(takeWhileFunc).Subscribe(begin =>
            {
                if (behaviour != null)
                {
                    onBegin.ComposeActions(begin).Invoke(begin);
                    io.OnMove.TakeUntil(io.OnEnd).Subscribe(move => onMove.ComposeActions(move).Invoke(move)).AddTo(disposables);
                    io.OnEnd.First().Finally(() =>
                    {
                        outerOnCompleted.Invoke();
                    }).Subscribe(end => onEnd.ComposeActions(end).Invoke(end)).AddTo(disposables);                    
                }
            }, () =>
            {
                onUnsubscribe.Invoke();
                ClearAll();
            });
        }

        public void FireOnBeginMessage(Action messageAction)
        {
            signalBus.Fire(new ObservableInputContextMessage.OnBegin()
            {
                messageAction = messageAction
            } as ObservableInputMessage);
        }

        public void FireOnActionMessage(Action messageAction)
        {
            signalBus.Fire(new ObservableInputContextMessage.OnAction()
            {
                messageAction = messageAction
            } as ObservableInputMessage);
        }

        public void FireOnCompleteMessage(Action messageAction)
        {
            signalBus.Fire(new ObservableInputContextMessage.OnComplete()
            {
                messageAction = messageAction
            } as ObservableInputMessage);
        }

        public void FireOnSubscribeMessage(Action messageAction)
        {
            signalBus.Fire(new ObservableInputContextMessage.OnSubscribe()
            {
                messageAction = messageAction
            } as ObservableInputMessage);
        }
    }
}
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UniRx;

namespace ZombieSoccer.Utils.IObservableExtensions
{
    public static class IObservableExtensions
    {
        public static IObservableExtensionsWrapper BuildPipeline(this IObservable<Unit> source)
        {
            return new IObservableExtensionsWrapper(source);
        }
    }

    public class IObservableExtensionsWrapper
    {
        private List<Func<bool>> _predicates = new List<Func<bool>>();
        private List<Action> _actions = new List<Action>();
        private List<Action> _failActions = new List<Action>();

        private IObservable<Unit> _source;
        private ReactiveProperty<bool> _reactiveProperty = new ReactiveProperty<bool>();

        private List<Action> _reactiveActions = new List<Action>();
        private List<Action> _failReactiveActions = new List<Action>();

        public IObservableExtensionsWrapper(IObservable<Unit> source)
        {
            this._source = source;
            _reactiveProperty.Value = true;
        }

        public IObservableExtensionsWrapper AddValidator<T>(Func<bool> predicate, params ReactiveProperty<T>[]  objectsOfObservation) where T : class, new()
        {
            this._predicates.Add(predicate);
            foreach (var observation in objectsOfObservation)
                observation.Where( obj => obj != null).Subscribe(obj => CheckPredicates() );
            return this;
        }

        public IObservableExtensionsWrapper AddAction(Action action, Action reactiveAction = null)
        {
            this._actions.Add(action);
            if(reactiveAction != null) this._reactiveActions.Add(reactiveAction);
            return this;
        }

        public IObservableExtensionsWrapper AddFailAction(Action action, Action reactiveAction = null)
        {
            this._failActions.Add(action);
            if (reactiveAction != null) this._failReactiveActions.Add(reactiveAction);
            return this;
        }

        public IDisposable ToSubscribe()
        {
            _reactiveProperty.Subscribe(_ =>
            {
                if (_reactiveProperty.Value)
                    _reactiveActions.ForEach(action => action.Invoke());
                else
                    _failReactiveActions.ForEach(failAction => failAction.Invoke());
            });
            return _source.Subscribe(_ => RunActions() );
        }

        private void RunActions()
        {
            if (_reactiveProperty.Value)
                _actions.ForEach(action => action.Invoke());
            else
                _failActions.ForEach(failAction => failAction.Invoke());
        }

        private void CheckPredicates()
        {
            bool valid = true;
            foreach (var predicate in _predicates)
            {
                valid = predicate.Invoke();
                if (!valid) break;
            }

            _reactiveProperty.Value = valid;
        }

    }
}

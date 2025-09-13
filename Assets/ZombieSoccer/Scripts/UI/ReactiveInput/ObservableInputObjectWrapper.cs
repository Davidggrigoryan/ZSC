using System;
using InputObservable;

namespace ZombieSoccer.ReactiveInput
{
    public static class ObservableInputObjectBuilder
    {
        public static ObservableInputObjectWrapper Create()
        {
            return new ObservableInputObjectWrapper();
        }
    }

    public class ObservableInputObjectWrapper
    {
        private Action<InputEvent> onAction;
        private Action beforeAction;
        private Action afterAction;

        public ObservableInputObjectWrapper AddOnAction(Action<InputEvent> onAction)
        {
            this.onAction = onAction;
            return this;
        }

        public ObservableInputObjectWrapper BeforeOnAction(Action beforeAction)
        {
            this.beforeAction = beforeAction;
            return this;
        }

        public ObservableInputObjectWrapper AfterOnAction(Action afterAction)
        {
            this.afterAction = afterAction;
            return this;
        }

        private Action<Action, Action<InputEvent>, Action, InputEvent> ComposeActionsFunc =
            (Action beforeAction, Action<InputEvent> onAction, Action afterAction, InputEvent e) => // ??????
            {
                if (beforeAction != null)
                {
                    beforeAction.Invoke();
                }

                onAction.Invoke(e);

                if (afterAction != null)
                {
                    afterAction.Invoke();
                }
            };

        public Action<InputEvent> ComposeActions(InputEvent e)
        {
            return (e) => ComposeActionsFunc(beforeAction, onAction, afterAction, e);
        }
    }
}

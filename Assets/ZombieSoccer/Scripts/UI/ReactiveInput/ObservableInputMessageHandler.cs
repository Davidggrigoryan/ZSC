using Zenject;

namespace ZombieSoccer.ReactiveInput
{
    public class ObservableInputMessageHandler : IInitializable
    {

        [Inject]
        protected SignalBus signalBus;

        public void Initialize()
        {
            signalBus.Subscribe<ObservableInputMessage>(ProcessInput);
        }

        protected virtual void ProcessInput(ObservableInputMessage inputMessage)
        {
            inputMessage.messageAction.Invoke();
        }

        public override string ToString()
        {
            return "this is message handler";
        }
    }
}
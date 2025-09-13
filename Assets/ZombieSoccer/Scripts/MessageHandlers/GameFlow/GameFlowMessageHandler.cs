using Zenject;
using ZombieSoccer.GameLayer.Flow;
using ZombieSoccer.ZombieSoccer.Scripts.Models.Messages.GameFlowNS;

namespace ZombieSoccer.ZombieSoccer.Scripts.MessageHandlers.GameFlowNS
{
    public class GameFlowMessageHandler : IInitializable
    {
        [Inject] 
        protected SignalBus _signalBus;

        private GameFlow _gameFlow;
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameFlowMessage>(ProcessInput);
        }
        
        protected virtual void ProcessInput(GameFlowMessage inputMessage)
        {
            if (inputMessage is MatchOutcomeGameFlowMessage matchOutcomeMessage)
            {
                if (matchOutcomeMessage.matchOutcome.IsWin.HasValue && matchOutcomeMessage.matchOutcome.IsWin.Value)
                {
                    _gameFlow.CalculatePins();
                    _gameFlow.OnAlliesWin();
                    
                }
            }
        }

        public void SetGameFlow(GameFlow gameFlow)
        {
            _gameFlow = gameFlow;
        }
    }
}
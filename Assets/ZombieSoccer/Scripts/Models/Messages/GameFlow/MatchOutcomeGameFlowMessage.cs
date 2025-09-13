using ZombieSoccer.Models.MatchM;

namespace ZombieSoccer.ZombieSoccer.Scripts.Models.Messages.GameFlowNS
{
    public class MatchOutcomeGameFlowMessage : GameFlowMessage
    {
        public MatchOutcome matchOutcome;

        public MatchOutcomeGameFlowMessage(MatchOutcome matchOutcome)
        {
            this.matchOutcome = matchOutcome;
        }
    }
}
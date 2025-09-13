using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieSoccer.Models.MatchM
{
    public class MatchOutcome
    {
        public List<MatchAction>? MatchLog { get; set; }

        public bool? IsWin { get; set; }

        public TeamJson? EnemyTeam { get; set; }

        public TeamJson? PlayerTeam { get; set; }

        public int? PlayerTeamPowerScore { get; set; }
        public int? EnemyTeamPowerScore { get; set; }
    }
}

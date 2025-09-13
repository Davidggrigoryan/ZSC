namespace ZombieSoccer.GameLayer.Flow
{
    public class MatchPreset
    {
        public int matchIndex;
        public int enemiesLevel;
        public string teamPositions;
        public int reward;
        public string enemiesIds;
        
        public string matchId;
        public MatchPreset(string matchId, int enemiesLevel, int matchIndex)
        {
            this.matchId = matchId;
            this.enemiesLevel = enemiesLevel;
            this.matchIndex = matchIndex;
        }
        
        public TeamPositionsPreset teamPositionsPreset => GetTeamPositionsPreset();

        public string[] chracters => enemiesIds.Split(',');

        private TeamPositionsPreset GetTeamPositionsPreset()
        {
            var tmp = teamPositions.Split('-');
            var preset = new TeamPositionsPreset();

            for (int i = 0; i < preset.charactersPerSegment.Length; i++)
                preset.charactersPerSegment[i] = int.Parse(tmp[i]);

            return preset;
        }
    }
}

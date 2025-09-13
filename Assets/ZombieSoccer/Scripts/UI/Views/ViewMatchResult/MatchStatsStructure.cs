using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using ZombieSoccer.Data;
using ZombieSoccer.GameLayer.Characters;


namespace ZombieSoccer
{
    public struct MatchStatsStructure
    {
        public bool IsDefeat;
        public List<CharacterPosition> PlayerCharacter, EnemyCharacters;
        public List<int> PlayerCharacterGoals, EnemyCharacterGoals;
        public List<Sprite> RewardsSprites;
        public int[] RewardsCounts;
        public int PlayerScore, EnemyScore;
    }
}
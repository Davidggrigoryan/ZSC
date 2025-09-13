using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSoccer
{
    [CreateAssetMenu(fileName = "TeamPositionsPresetsCollection", menuName = "TeamPositionsPresetsCollection", order = 52)]
    public class TeamPositionsPresetsCollection : ScriptableObject
    {
        [SerializeField]
        public List<TeamPositionsPreset> teamPositions = new List<TeamPositionsPreset>();

        [Button]
        public string ToText()
        {
            var result = string.Empty;

            foreach (var preset in teamPositions)
            {
                result += preset.ToString() + ",";
            }

            return result;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZombieSoccer
{
    [System.Serializable]
    public class TeamPositionsPreset
    {
        [SerializeField]
        public int[] charactersPerSegment = new int[3];

        public int[] Reverse => charactersPerSegment.Reverse().ToArray();

        public override string ToString()
        {
            return $"{charactersPerSegment[0]}-{charactersPerSegment[1]}-{charactersPerSegment[2]}";
        }
    }

    [System.Serializable]
    public class TeamSetup
    {
        public int presetIndex= -1;

        [SerializeField]
        public string[] characters = new string[7];
    }
}

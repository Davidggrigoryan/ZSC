using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSoccer.Models.MatchM
{
    [Serializable]
    public class TeamNode
    {
        [SerializeField]
        public string Id { get; set; }
        [SerializeField]
        public string Pos { get; set; }

        public TeamNode(string Id, string Pos)
        {
            this.Id = Id;
            this.Pos = Pos;
        }
    }

    [Serializable]
    public class TeamJson
    {
        [SerializeField]
        public List<TeamNode> Team = new List<TeamNode>();

    }
    
}

using UnityEngine;
using ZombieSoccer.GameLayer.Characters;

namespace ZombieSoccer.GameLayer.UI
{
    public interface ICharacterView 
    {
        public Character character { get; set; }
        public TeamType teamType { get; set; }
        public Transform GetTransform();
        public GameObject GetGameObject();
    }
}

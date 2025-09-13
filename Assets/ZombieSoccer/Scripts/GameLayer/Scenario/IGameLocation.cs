using UnityEngine;

namespace ZombieSoccer.GameLayer.Flow
{
    public abstract class IGameLocation : ScriptableObject
    {
        protected GameObject _gameObject;
        public MapTypeEnum mapType;
        public abstract GameObject GetGameObject();

        public abstract void SetGameObject(GameObject gameObject);
        
        public virtual void Hide()
        {
            _gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            _gameObject.SetActive(true);
        }
    }
}
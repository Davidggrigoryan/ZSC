using UnityEngine;

namespace ZombieSoccer.DebugMenu
{
    public abstract class DebugMenuPage : MonoBehaviour
    {

        [SerializeField] protected GameObject Page;

        public abstract void Open();

        public abstract void Close();        
        
    }
}

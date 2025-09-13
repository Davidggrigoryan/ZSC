using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer.DebugMenu
{
    public class ActiveLayer : DebugMenuPage
    {
        [SerializeField] Button DebugOpen;        

        public override void Close()
        {            
            base.Page.SetActive(false);
        }

        public override void Open()
        {
            base.Page.SetActive(true);            
        }
    }
}

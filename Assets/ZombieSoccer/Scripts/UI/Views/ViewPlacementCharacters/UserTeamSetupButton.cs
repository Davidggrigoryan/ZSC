using System;
using UnityEngine;
using UnityEngine.UI;
//using ZombieSoccer.Data;
using ZombieSoccer.Utitlies;

namespace ZombieSoccer.GameLayer.UI
{
    public class UserTeamSetupButton : MonoBehaviour
    {
        //[SerializeField] private GameObject bottom;
        
        public void Awake()
        {
            this.GetComponent<Button>().onClick.AddListener( () => OnClick() );
            //this.bottom.SetActive( true );
            //FieldPresetManager.userTeamSetupButton = this;

        }

        private void Start()
        {
            OnClick(0);
        }


        public Action<int> UserTeamSetupEventHandler;
        
        public virtual void OnClick(int arg = 0)
        {
            UserTeamSetupEventHandler?.Invoke(arg);
        }
    }
}

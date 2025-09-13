using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer.UI.Views
{
    public class ViewPlayerSettings : MonoBehaviour
    {
        [SerializeField]
        private Slider SoundOffButton;
        //private UIPopup myPopup;
        public static System.Action SoudTurnedOff;
        public static System.Action SoudTurnedOn;
        public static System.Action LanguageSwiched;
        public static System.Action ConnectoinToFB;
        public static System.Action SupportCalled;
        [SerializeField]
        private Button SwicherLanguage;
        [SerializeField]
        private Button Support;
        [SerializeField]
        private Button ConnectFB;

        private void Start()
        {
            SwicherLanguage.onClick.AddListener(SwichLanguage);
            Support.onClick.AddListener(CallSuport);
            ConnectFB.onClick.AddListener(ConnectToFB);
        }
        public void SwichLanguage()
        {
            LanguageSwiched?.Invoke();
            Debug.Log("LanguageSwiched");
        }
        public void ConnectToFB()
        {
            ConnectoinToFB?.Invoke();
            Debug.Log("Request connect to FB");
        }

        public void CallSuport()
        {
            SupportCalled?.Invoke();
            Debug.Log("Request to support");
        }
        public void SoundOnOff()
        {
            if (SoundOffButton.value == 0)
            {
                SoundOffButton.value = 1;
                SoudTurnedOn?.Invoke();
                Debug.Log("SoundOn");

            }
            else
            {
                SoundOffButton.value = 0;
                SoudTurnedOff?.Invoke();
                Debug.Log("SoundOff");
            }
        }
    }
}

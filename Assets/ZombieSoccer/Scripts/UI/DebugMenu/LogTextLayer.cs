using Cysharp.Threading.Tasks;
using System.Text;
using TMPro;
using UnityEngine;

namespace ZombieSoccer.DebugMenu
{
    public class LogTextLayer : DebugMenuPage
    {
        [SerializeField] GameObject TextLayer;

        [SerializeField] GameObject Root;

        StringBuilder logBuilder = new StringBuilder();        
        
        bool _active = false;

        private void OnEnable()
        {
            Debug.Log("start log receiver ");
            Application.logMessageReceived += HandleLog;
        }

        public override void Close()
        {
            Root.SetActive(false);
            _active = false;
            TextLayer.GetComponent<TextMeshProUGUI>().text = string.Empty;
        }

        public override void Open()
        {
            Root.SetActive(true);
            
            _active = true;
            
            TextLayer.GetComponent<TextMeshProUGUI>().text = logBuilder.ToString();

            CorrectSize();
        }
        
        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            string newString = "\n [" + type + "] : " + logString;
            
            if (type == LogType.Exception)
            {
                newString = "\n" + stackTrace;
            }            

            logBuilder.Append(newString);
            
            if ( true == _active )
                TextLayer.GetComponent<TextMeshProUGUI>().text += newString;

            CorrectSize();
        }

        private async void CorrectSize()
        {
            await UniTask.Yield();
            TextLayer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, TextLayer.GetComponent<TextMeshProUGUI>().GetPreferredValues().y);
        }

        public void ClearFunc()
        {
            TextLayer.GetComponent<TextMeshProUGUI>().text = string.Empty;
            logBuilder.Clear();
            CorrectSize();
        }
    }
}

using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using UnityEngine.UI.Extensions;

namespace ZombieSoccer.Localization
{
    public static class LocalizeDecorator
    {
        public static LocalizationManager LocalizationManager { private get; set; }

        public static void Localize(this TextMeshProUGUI textMesh, string tableName, string keyName)
        {
            var localizeStringEvent = textMesh.transform.gameObject.GetOrAddComponent<LocalizeStringEvent>();
            localizeStringEvent.StringReference = LocalizationManager.GetLocalizedString(tableName, keyName);
            
            var unityEventString = new UnityEventString();
            unityEventString.AddListener((text) => textMesh.text = text);
            localizeStringEvent.OnUpdateString = unityEventString;
        }

    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer.UI
{

    [CreateAssetMenu(fileName = "MainButtonElementsScriptableObject", menuName = "Utils/MainButtonElements")]
    public class MainButtonElements : ScriptableObject
    {
        [SerializeField]
        Sprite[] activeSprites;

        [SerializeField]
        Color activeTextColor;

        [SerializeField]
        Sprite[] inactivateSprites;

        [SerializeField]
        Color inactiveTextColor;

        public void SetState(Transform mainButtonRoot, bool active, bool activityMapping = true)
        {
            Image[] components = mainButtonRoot.GetComponentsInChildren<Image>();
            TextMeshProUGUI label = mainButtonRoot.GetComponentInChildren<TextMeshProUGUI>();

            if (active)
            {
                for (int i = 0; i < activeSprites.Length; i++)
                    components[i].sprite = activeSprites[i];                
                label.color = activeTextColor;
            }
            else
            {
                for (int i = 0; i < inactivateSprites.Length; i++)
                    components[i].sprite = inactivateSprites[i];
                label.color = inactiveTextColor;
            }
            
            if(activityMapping)
                mainButtonRoot.GetComponentInChildren<Button>().enabled = active;
        }
    }
}

using TMPro;
using UnityEngine;

namespace ZombieSoccer.UI
{
    [CreateAssetMenu(fileName = "TextRarityColors", menuName = "Utils/TextRarityColors")]
    public class TextRarityColors : ScriptableObject
    {
        public Color[] textRarityColors;

        public void UpdateTextColor(TextMeshProUGUI textMesh, int rarity)=>
            textMesh.color = textRarityColors[CharacterAttributes.GetIndexForRarityElement(rarity)];
    }
}

using UnityEngine;

namespace ZombieSoccer
{
    [CreateAssetMenu(fileName = "ColorsPalleteScriptableObject", menuName = "Utils/ColorPallete")]
    public class ColorsPallete : ScriptableObject
    {
        public Color[] charactersRarityColors;
    }
}

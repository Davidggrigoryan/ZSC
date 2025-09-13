using UnityEngine;

namespace ZombieSoccer
{
    [CreateAssetMenu(fileName = "ShieldPalleteScriptableObject", menuName = "Utils/ShieldPallete")]
    public class ShieldPalleteScriptableObject : ScriptableObject
    {
        public Sprite[] shieldsSprites, detailsSprites, tapesSprites, ballsSprites;
    }
}

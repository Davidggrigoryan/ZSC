using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer
{
    [CreateAssetMenu(fileName = "CardBordersScriptableObject", menuName ="Utils/CardBorders" )]
    public class CardBorder : ScriptableObject
    {
        public Sprite[] borders;
    }
}

using UnityEngine;
using UnityEngine.UI;
using ZombieSoccer.GameLayer.Characters;

namespace ZombieSoccer.UI
{
    [CreateAssetMenu(fileName = "CharacterAttributesScriptableObject", menuName = "Utils/CharacterAttributes")]
    public class CharacterAttributes : ScriptableObject
    {
        public Sprite[] backgrounds;
        public Sprite[] slotBackgroundIcons;
        public Sprite[] rarityIcons;
        public Sprite[] animalIcons;
        public Sprite[] elementIcons;
        public Sprite[] roleIcons;

        static public int GetIndexForRarityElement(int rarity) =>
            rarity == 1 ? 0 : rarity / 2;

        public void UpdateBackgroundOnRarity(Image background, int rarity) =>
            background.sprite = backgrounds[GetIndexForRarityElement(rarity)];        

        public void UpdateSlotBackground(Image icon, int rarity) => 
            icon.sprite = slotBackgroundIcons[GetIndexForRarityElement(rarity)];

        public void UpdateIconOnRarity(Image icon, int rarity) =>
            icon.sprite = rarityIcons[rarity - 1];

        public void UpdateAnimal(Image icon, AnimalsEnum animalsEnum) =>
            icon.sprite = animalIcons[(int)animalsEnum];

        public void UpdateElement(Image icon, ElementsEnum elementsEnum) =>
            icon.sprite = elementIcons[(int)elementsEnum];

        public void UpdateCharacterRole(Image icon, CharacterRole characterRole) =>
            icon.sprite = roleIcons[(int)characterRole - 1];

    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ZombieSoccer.GameLayer.UI
{
    public class CharacterViewGraphic : MonoBehaviour
    {
        [Inject]
        private ColorsPallete colorPallete;

        [SerializeField]
        private Image[] coloredImages;     

        [SerializeField]
        private Image[] stars;

        [SerializeField]
        private Sprite activeStarSprite;

        [SerializeField]
        private Image backImage;

        //public Color allyColor, enemyColor;        

        public void UpdateStars(int count)
        {
            // has different on character model betwen front and back
            try
            {
                for (int i = 0; i < count; i++)
                    stars[i].sprite = activeStarSprite;
            }
            catch(Exception e)
            {
            }            
        }

        public void UpdateColors()
        {
            var color = colorPallete.charactersRarityColors[UnityEngine.Random.Range(0, colorPallete.charactersRarityColors.Length)];
            foreach (var image in coloredImages)
                image.color = color;
        }

        public void UpdateColors(Color color)
        {
            foreach (var image in coloredImages)
                image.color = color;
        }

        public void UpdateColors(int rarity)
        {
            int idxColor = 0;
            
            if (rarity == 1)
                idxColor = 0;
            else if (rarity == 2 || rarity == 3)
                idxColor = 1;
            else if (rarity == 4 || rarity == 5)            
                idxColor = 2;                            
            else if (rarity == 6 || rarity == 7)
                idxColor = 3;
            else if (rarity == 8)
                idxColor = 4;            

            foreach (var image in coloredImages)
                image.color = colorPallete.charactersRarityColors[ idxColor ];
            backImage.color = colorPallete.charactersRarityColors[ idxColor ];
            //Material mat = new Material(backImage.material);
            //mat.enableInstancing = true;
            //mat.SetColor("_MainColor", colorPallete.characterViewColors[idxColor]);
            //backImage.material = mat;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.GameLayer.UI;

namespace ZombieSoccer.Extensions
{
    public static class GridLayoutExtensions
    {
        public static List<DetailCharacterView> SpawnDetailCharacterViews(this GridLayoutGroup gridLayout, List<Character> characters, DetailCharacterView.Factory detailCharacterViewFactory, Action<Character> ButtonHandler )
        {
            List<DetailCharacterView> l = new List<DetailCharacterView>();

            characters.ForEach( character =>
            {
                var view = detailCharacterViewFactory.Create(character, TeamType.Ally, gridLayout.transform);
                //view.transform.SetParent(gridLayout.transform, false);

                var button = view.GetGameObject().GetOrAddComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => ButtonHandler(character));

                l.Add(view);
            });
            return l;
        }
        
        public static void Prepare(this GridLayoutGroup gridLayout)
        {
            //gridLayout.transform.DestroyChilds();

            var gridWidth = gridLayout.GetComponent<RectTransform>().rect.width;
            var cellHeight = gridWidth / (4f / 3f + 3f * 0.85F);
            var cellOffset = (int)(cellHeight / 3f);

            gridLayout.cellSize = new Vector2(224, 286);
            gridLayout.padding.left = cellOffset;
            gridLayout.padding.right = cellOffset;
            gridLayout.spacing = new Vector2(70, 100);
        }        
    }
}

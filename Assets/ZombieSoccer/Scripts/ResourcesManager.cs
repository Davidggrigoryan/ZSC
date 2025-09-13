using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using ZombieSoccer.GameLayer.Characters;

namespace ZombieSoccer.ApplicationLayer.Data
{
    public class ResourcesManager : IInitializable
    {
        public class CharacterSpritesData
        {
            public CharacterSpritesData(in Sprite icon, in Sprite fullBody, in Sprite activeSkillSprite, in Sprite passiveSkillSprite)
            {
                Icon = icon;
                FullBody = fullBody;
                ActiveSkillSprite = activeSkillSprite;
                PassiveSkillSprite = passiveSkillSprite;
            }

            public Sprite Icon { get; }
            public Sprite FullBody { get; }
            public Sprite ActiveSkillSprite { get; }
            public Sprite PassiveSkillSprite { get; }
        }

        public Dictionary<string, CharacterSpritesData> CharactersSprites { get; private set; }

        public Dictionary<AnimalsEnum, Sprite> AnimalsIcons { get; private set; }
        public Dictionary<ElementsEnum, Sprite> ElementsIcons { get; private set; }

        public Dictionary<string, Sprite> Sprites { get; private set; } = new Dictionary<string, Sprite>();

        public void Initialize()
        {
            CharactersSprites = new Dictionary<string, CharacterSpritesData>();

            LoadAnimalsIcons();
            LoadElementsIcons();
        }

        private void LoadAnimalsIcons()
        {
            AnimalsIcons = new Dictionary<AnimalsEnum, Sprite>();
            AnimalsIcons.Add(AnimalsEnum.Beaver, Resources.Load<Sprite>(Path.Combine(CommonStrings.PathAnimalsIcons, "Beaver")));
            AnimalsIcons.Add(AnimalsEnum.Boar, Resources.Load<Sprite>(Path.Combine(CommonStrings.PathAnimalsIcons, "Boar")));
            AnimalsIcons.Add(AnimalsEnum.Penguin, Resources.Load<Sprite>(Path.Combine(CommonStrings.PathAnimalsIcons, "Penguin")));
            AnimalsIcons.Add(AnimalsEnum.Woodpecker, Resources.Load<Sprite>(Path.Combine(CommonStrings.PathAnimalsIcons, "Woodpecker")));
        }

        private void LoadElementsIcons()
        {
            ElementsIcons = new Dictionary<ElementsEnum, Sprite>();
            ElementsIcons.Add(ElementsEnum.Air, Resources.Load<Sprite>(Path.Combine(CommonStrings.PathElementsIcons, "Air")));
            ElementsIcons.Add(ElementsEnum.Earth, Resources.Load<Sprite>(Path.Combine(CommonStrings.PathElementsIcons, "Earth")));
            ElementsIcons.Add(ElementsEnum.Fire, Resources.Load<Sprite>(Path.Combine(CommonStrings.PathElementsIcons, "Fire")));
            ElementsIcons.Add(ElementsEnum.Water, Resources.Load<Sprite>(Path.Combine(CommonStrings.PathElementsIcons, "Water")));
        }

        public CharacterSpritesData GetCharacterSprites(in Character character)
        {
            if (CharactersSprites.ContainsKey(character.archetypeId))
            {
                return CharactersSprites[character.archetypeId];
            }
            else
            {
                var folderPath = Path.Combine(CommonStrings.PathCharactersSprites, $"{character.portalName}/{character.characterName}");
                var icon = Resources.Load<Sprite>(Path.Combine(folderPath, "Icon"));
                var fullBody = Resources.Load<Sprite>(Path.Combine(folderPath, "FullBody"));
                var activeSkillSprite = Resources.Load<Sprite>(Path.Combine(folderPath, "ActiveSkillSprite"));
                var passiveSkillSprite = Resources.Load<Sprite>(Path.Combine(folderPath, "PassiveSkillSprite"));
                var data = new CharacterSpritesData(icon, fullBody, activeSkillSprite, passiveSkillSprite);                
                
                CharactersSprites.Add(character.archetypeId, new CharacterSpritesData(icon, fullBody, activeSkillSprite, passiveSkillSprite));

                return data;
            }
        }

        public async UniTask<Sprite> GetSprite(string path, string spriteName)
        {
            if( Sprites.ContainsKey(spriteName))
                return Sprites[spriteName];
            ResourceRequest resourceRequest = Resources.LoadAsync<Sprite>(Path.Combine(path, spriteName));
            Sprite sprite = await resourceRequest as Sprite;
            Sprites.Add( spriteName, sprite );
            return sprite;
        }

    }
}
using System.Linq;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.Models.DeckM;

namespace ZombieSoccer.CharactersMergeController
{
    public class CharactersMerge
    {

        [Inject]
        DeckModel deckModel;

        public bool IsValidForMerge(Character mainCharacter, Character usingCharacter)
        {
            
            if (mainCharacter.instanceId == usingCharacter.instanceId) return false;

            switch (mainCharacter.rarity)
            {
                case RarityEnum.Rare:
                    if (usingCharacter.rarity == mainCharacter.rarity
                        &&
                        usingCharacter.archetypeId == mainCharacter.archetypeId)
                        return true;
                    break;
                case RarityEnum.RarePlus:
                    if (usingCharacter.rarity == mainCharacter.rarity)
                        return true;
                    break;
                case RarityEnum.Epic:
                    if (usingCharacter.rarity == mainCharacter.rarity
                        &&
                        usingCharacter.archetypeId == mainCharacter.archetypeId)
                        return true;
                    break;
                case RarityEnum.EpicPlus:
                    if (usingCharacter.rarity == mainCharacter.rarity)
                        return true;
                    break;
                case RarityEnum.Legendary:
                    if (usingCharacter.rarity == RarityEnum.EpicPlus
                        &&
                        usingCharacter.archetypeId == mainCharacter.archetypeId)
                        return true;
                    break;
                case RarityEnum.LegendaryPlus:
                    if (usingCharacter.rarity == mainCharacter.rarity)
                        return true;
                    break;
            }

            return false;
        }

        /*
       rarity -> rarity + rarity => rarity+
       rarity+ -> rarity+: Any + rarity+: Any => Epic
       epic -> epic => epic+

       epic+ -> epic+: Any + epic+: Any => Legend
       legend -> epic+ => legend+
       legend+ -> legend+: Any => Genius
       */

        public Character[] GetValidCharacters(Character mainCharacer)
        {            
            int mainRarity = (int)CharactersManager.DefaultCharacters.data.people[mainCharacer.archetypeId].rarity;

            var characters = deckModel.Characters.data.Values.ToList().Where(x => IsAvailableRarity(mainRarity, (int)x.rarity)).Select(x => x).ToArray();

            return characters;
        }

        public bool IsAvailableRarity(int mainRarity, int mergedRarity)
        {
            if (mainRarity >= 2 && mainRarity <= 4)
            {
                if (mergedRarity >= 2 && mergedRarity <= 4)
                    return true;
                return false;
            }
            if (mainRarity >= 5 && mainRarity <= 9)
            {
                if (mergedRarity >= 5 && mergedRarity <= 9)
                    return true;
                return false;
            }
            return false;
        }
        
        public Character Merge( Character mainCharacter, Character[] usingCharacters)
        {
            mainCharacter.rarity++;
            return mainCharacter;
        }        
    }
}

using ZombieSoccer.GameLayer.Characters;

namespace ZombieSoccer.Models.DeckM
{
    public class DefaultDeckSort : IDeckSort<Character>
    {
        public int Compare(Character x, Character y)
        {
            if (x.rarity < y.rarity)
                return -1;
            else if (x.rarity == y.rarity)
                return 0;
            return 1;
        }
    }
}

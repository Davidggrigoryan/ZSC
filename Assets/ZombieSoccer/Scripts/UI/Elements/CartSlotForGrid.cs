using UnityEngine;
using Zenject;

namespace ZombieSoccer.GameLayer.UI
{
    public class CartSlotForGrid : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, CartSlotForGrid>
        {

        }
    }
}

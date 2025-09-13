using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSoccer
{
    public class UpdateRarityPanel : MonoBehaviour
    {
        public GameObject CurrentCart, LeftCartSlot, RightCartSlot, ResultCart;
        public System.Action CartsAdded, CartsRemoved;

        private void Start()
        {
            CartsAdded?.Invoke();
        }


    }
}

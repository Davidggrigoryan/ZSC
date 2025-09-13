using System;
using UnityEngine;
using Zenject;
using ZombieSoccer.GameLayer.Flow;

namespace ZombieSoccer.ZombieSoccer.Scripts.Locations.Pin
{
    public class Pin : MonoBehaviour 
    {
        [Inject]
        public void Construct()
        {
        }

        public class Pool : MonoMemoryPool<Pin>
        {
            
        }
    }
}
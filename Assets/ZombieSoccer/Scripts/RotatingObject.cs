using InputObservable;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Zenject;

namespace ZombieSoccer
{

    public class RotatingObjectSignal
    {
        public bool Show { get; set; }
    }

    public class RotatingObject : MonoBehaviour
    {        

        [Inject]
        SignalBus _signalBus;

        private void OnEnable()
        {
            _signalBus.Subscribe<RotatingObjectSignal>(RotatingObjectSignalReceiver);
            
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<RotatingObjectSignal>(RotatingObjectSignalReceiver);
        }

        private void RotatingObjectSignalReceiver(RotatingObjectSignal rotatingObjectSignal)
        {
            this.gameObject.SetActive(rotatingObjectSignal.Show);
        }


    }
}

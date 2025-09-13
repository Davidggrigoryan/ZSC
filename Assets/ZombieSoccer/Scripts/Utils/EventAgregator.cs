using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZombieSoccer
{
    public class EventAgregator
    {
        public static WorldEvent World = new WorldEvent();

        public static void ResetALL()
        {
            World.Cleare();
        }
    }

    public class WorldEvent
    {
        private Dictionary<string, UnityEvent> ListenersEventConsumable = new Dictionary<string, UnityEvent>();

        public void AddEventListenerConsumable(string EventName, UnityAction You_Delegate)
        {
            if (ListenersEventConsumable.ContainsKey(EventName))
            {
                ListenersEventConsumable[EventName].AddListener(You_Delegate);
            }
            else
            {
                ListenersEventConsumable.Add(EventName, new UnityEvent());
                ListenersEventConsumable[EventName].AddListener(You_Delegate);
            }
        }

        public void PublichEvent(string EventName)
        {
            if (ListenersEventConsumable.ContainsKey(EventName))
                ListenersEventConsumable[EventName].Invoke();
        }

        public void Cleare()
        {
            ListenersEventConsumable = new Dictionary<string, UnityEvent>();
        }
    }

}

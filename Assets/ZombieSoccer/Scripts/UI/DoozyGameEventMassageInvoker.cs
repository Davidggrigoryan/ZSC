using Doozy.Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSoccer
{
    public class DoozyGameEventMassageInvoker : MonoBehaviour
    {
        public void InvokeMessage(string message)
        {
            GameEventMessage.SendEvent(message);
        }
    }
}

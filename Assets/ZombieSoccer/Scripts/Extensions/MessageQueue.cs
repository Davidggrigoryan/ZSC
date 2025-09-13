using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

namespace ZombieSoccer.Extensions
{
    public static class MessageQueue
    {
        public static void DefineDispatch(Action action, int milliseconds)
        {
            new Dispatch(action, milliseconds);
        }
    }

    public class Dispatch
    {

        public static async void CreateDispatchAsync(Action action, int milliseconds)
        {
            await UniTask.Delay(milliseconds);
            action();
        }

        public Dispatch(Action action, int milliseconds)
        {
            Dispatch.CreateDispatchAsync(action, milliseconds);
        }
    }
}

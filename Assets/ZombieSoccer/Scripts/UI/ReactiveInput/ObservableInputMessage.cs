using System;

namespace ZombieSoccer.ReactiveInput
{
    public abstract class ObservableInputMessage
    {
        public string msg;
        public Action messageAction;
    }
}
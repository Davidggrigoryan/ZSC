using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.UI.Popups;
using ZombieSoccer.UI.Presenters;

namespace ZombieSoccer.UI
{
    public enum TargetType { VIEW, POPUP };
    
    public enum MessageNotify
    {       
        None,
        UnknownError,
        NoNetworkError,
        InvalidEmail,
        InvalidPassword,
        
        // character view
        CharacterListEmpty
    }

    public class OpenUIViewPageSignal
    {
        public string targetPage { get; set; }

        public System.Object[] args { get; set; }
    }

    public class InititalizeUISignal { }

    public class OpenUIPopUpSignal
    {
        public MsgTypeEnum msgType { get; set; }
        public string msg { get; set; }
        public Action callback { get; set; }
    }

    public class PortalPopupSignal
    {
        public Character character;                
    }

    public class CharacterMergePopupSignal
    {
        public Character character;
        public Action action;
    }

    public enum MsgTypeEnum
    {
        Normal,
        Warning,
        Error
    }

    public class PageManager : IInitializable
    {
        [Inject]
        PageManagerConfigScriptableObject pageManagerConfig;

        [Inject]
        private DiContainer container { get; set; }

        [Inject]
        private SignalBus signalBus;       

        private static List<string> setups = new List<string>();
        private static List<string> pages = new List<string>();

        public async void Initialize()
        {
            var parent = GameObject.Find("SafeArea").transform;

            var pages = new List<GameObject>();
            pageManagerConfig.setupPages.ForEach(x =>
            {
                var page = container.InstantiatePrefab(x, parent);
                page.GetComponent<PageSignalHandler>().Setup(x.name);
                pages.Add(page);
            });

            signalBus.Fire(new InititalizeUISignal());

            await UniTask.Yield();

            foreach (var page in pages)
            {
                await UniTask.Yield();
                Fire(page.name);
                await UniTask.Yield();
                await page.GetComponent<BasePagePresenter>().task;
            }
            pages.Clear();
            Debug.LogError("SETUP DONE");

            pageManagerConfig.appPages.ForEach(x =>
                container.InstantiatePrefab(x, parent).GetComponent<PageSignalHandler>().Setup(x.name));

            signalBus.Fire(new InititalizeUISignal());
            await UniTask.Yield();
            Fire(pageManagerConfig.appPages[0].name);
        }

        public void Fire(string targetPage, params System.Object[] args)
        {
            signalBus.Fire(new OpenUIViewPageSignal()
            {
                targetPage = targetPage,
                args = args
            });
        }

        public void FireMessage(MsgTypeEnum msgType, string msg)
        {

            signalBus.Fire(new OpenUIPopUpSignal()
            {
                msgType = msgType,
                msg = msg
            });
        }

        public void FirePortalPopup(Character character )
        {
            signalBus.Fire(new PortalPopupSignal()
            {
                character = character,                
            });
        }

        public void FireNotifyingPopup(NotifyingPopupEnum notifyingPopupEnum)
        {
            signalBus.Fire(new NotifyingPopupSignal()
            {
                notifyingPopupEnum = notifyingPopupEnum
            });
        }

        public void FireDisintegratePopup(Action action, int dust, int elixir)
        {
            signalBus.Fire(new DisintegratePopupSignal()
            {
                action = action,
                dust = dust,
                elixir = elixir
            });
        }

        public void FireToShopMovePopup(ForActionNotEnoughEnum forActionNotEnoughEnum)
        {
            signalBus.Fire(new ToShopMovePopupSignal()
            {
                forActionNotEnoughEnum = forActionNotEnoughEnum
            });
        }

        public void FireUpgradeCharacterPopup(Action action)
        {
            signalBus.Fire(new UpgradeCharacterPopupSignal()
            {
                action = action
            });
        }
        
        public void FireStateSelectPopup(StateSelectPopupEnum stateSelectPopupEnum, Action button1Action, Action button2Action)
        {
            signalBus.Fire(new StateSelectPopupSignal()
            {
                stateSelectPopupEnum = stateSelectPopupEnum,
                button1Action = button1Action,
                button2Action = button2Action
            });
        }

        public void FireStorePopup(ForActionNotEnoughEnum e)
        {
            signalBus.Fire(new StorePopupSignal()
            {
                forActionNotEnoughEnum = e
            });
        }
    }
}

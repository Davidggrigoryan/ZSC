using Doozy.Engine.UI;
using System;
using TMPro;
//using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.Rendering;

namespace ZombieSoccer.UI
{
    public class UIPopUp: MonoBehaviour, IInitializable
    {
        [SerializeField] UIPopup doozyPopUpComponent;
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI text;
        //[SerializeField] LocalizeStringEvent localizeStringEvent;

        [Inject]
        protected PageManager pageManager;

        [Inject]
        protected SignalBus signalBus;

        [Inject]
        RenderManager renderManager;

        //[Inject]
        //LocalizationManager localizationManager;


        private Action DefaultActionOnClick => () => { /*pageManager.Fire(pageManager.MainPageName, null);*/ };
        //private bool _isActive = false;

        //private int BG_ChildIndex = 0, CONTAINTER_ChildIndex = 1;

        public virtual async void Show(OpenUIPopUpSignal payload)
        {
            //if (_isActive)
            //    return;
            //doozyPopUpComponent = UIPopup.GetPopup(payload.targetPage);
            text.text = payload.msg;
            button.onClick.RemoveAllListeners();
            var callback = payload.callback ?? DefaultActionOnClick;
            button.onClick.AddListener(new UnityAction(callback));
            button.onClick.AddListener(new UnityAction(()=>doozyPopUpComponent.Hide()));

            //localizeStringEvent.StringReference = await localizationManager.GetLocalizedString(MessageCollection, payload.ToString());
            doozyPopUpComponent.Show();
            
            //_isActive = true;
            //var container = doozyPopUpComponent.transform.GetChild( CONTAINTER_ChildIndex);
            //container.GetComponentInChildren<Button>().onClick.AddListener(() =>
            //{
            //    doozyPopUpComponent.Hide();
            //    doozyPopUpComponent = null;
            //    //_isActive = false;
            //});
            
           // container.GetComponentInChildren<LocalizeStringEvent>().StringReference = await localizationManager.GetLocalizedString(MessageCollection, payload.message.ToString());
        }

        public void Awake()
        {            
            signalBus.Subscribe<OpenUIPopUpSignal>(Show);

            renderManager.ResolveLayer<RenderLayerVFXOverlay>().SetTransformIntoContainer(this.transform);
            doozyPopUpComponent.Hide(false);
        }
        public void Initialize()
        {
            
        }
    }
}

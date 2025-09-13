using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieSoccer.UI.Widget
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class WidgetNavigationButton : WidgetBase
    {
        [SerializeField]
        protected bool isClickable;

        [ShowIf("isClickable", true)]
        [SerializeField]
        protected string pageName;

        protected override void Inititalize()
        {
            base.Inititalize();

            var button = gameObject.GetComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.interactable = isClickable;

            if (isClickable)
            {
                button.OnClickAsObservable().Subscribe(_ =>
                    GetComponentInParent<PageSignalHandler>().Fire(pageName));
            }
        }
    }
}

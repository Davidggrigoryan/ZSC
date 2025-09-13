using System.Threading.Tasks;
using UnityEngine;
using ZombieSoccer.Utils.RectTransformExtensions;

namespace ZombieSoccer.Utils.ListOfViewExtensions
{
    public static class ListOfViewsExtension
    {

        public static void DeactivateNonIntersectViews<TView>(Transform viewport) where TView : MonoBehaviour
        {
            Parallel.Invoke(() =>
            {
                var views = viewport.GetComponentsInChildren<TView>();
                foreach (var view in views)
                    if (!view.GetComponent<RectTransform>().Overlaps(viewport.GetComponent<RectTransform>()))
                        view.transform.GetChild(0).gameObject.SetActive(false);
                    else
                        view.transform.GetChild(0).gameObject.SetActive(true);
            });
        }
    }
}

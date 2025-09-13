//using UnityEngine;

//namespace ZombieSoccer.GameLayer.UI
//{
//    public class CardsDeckDragAndDropTarget : MonoBehaviour, IDragAndDropTarget
//    {
//        [SerializeField]
//        private Transform contentTransform;

//        public bool IsBusy { get; } = false;

//        public Transform GetRoot => contentTransform;

//        public void ResetTarget(CharacterView characterView)
//        {
//        }

//        public void SetCharacterViewToTarget(CharacterView value)
//        {
//            value.transform.SetParent(contentTransform);
//            value.transform.localPosition = Vector3.zero;
//        }
//    }
//}

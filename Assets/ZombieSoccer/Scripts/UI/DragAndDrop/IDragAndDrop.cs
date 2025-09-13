using UnityEngine;

namespace ZombieSoccer.UI.DragAndDrop
{
    public interface IDragAndDrop<T> where T : MonoBehaviour
    {
        public void SetObjectToTarget( T obj );

        public T GetObjectFromTarget();

        public void ResetTarget();
    }
}

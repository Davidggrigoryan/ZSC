using InputObservable;

namespace ZombieSoccer.ReactiveInput
{
    public interface ReactiveDraggable
    {
        public void Begin(InputEvent e);
        public void Dragging(InputEvent e);
        public void DragEnd(InputEvent e);
        public bool TakeWhile(InputEvent e);
        public void OnComplete();
        public void OnUnsubscribe();
        public void CreateInputContext();
        public void InitDraggableVariables();
    }
}
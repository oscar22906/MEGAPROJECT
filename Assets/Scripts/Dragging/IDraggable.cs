public interface IDraggable : IInteractable
{
    void OnDragBegin();
    void OnDragEnd();
    bool isDraggable();
}
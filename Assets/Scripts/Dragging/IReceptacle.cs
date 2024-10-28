public interface IReceptacle
{
    bool CanAccept(IDraggable draggable);

    void OnItemRecieve(IDraggable draggable);
}

using UnityEngine;

public class Container : MonoBehaviour, IReceptacle
{
    [SerializeField] private bool acceptAll;
    public bool CanAccept(IDraggable draggable)
    {
        return acceptAll;
    }

    public void OnItemRecieve(IDraggable draggable)
    {
        Debug.Log("Item successfully dropped into the container!");
    }
}

using UnityEngine;

public class GenericDial : MonoBehaviour, IDial
{
    [SerializeField] public float Value { get; private set; }
    public float MinRotation = 0f;
    public float MaxRotation = 360f;
    bool isInteractable;

    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private bool useHorizontalDrag = false; // If false, use vertical drag

    public bool isDraggable() { return true; }

    public void OnDialDrag(float deltaX, float deltaY)
    {
        float dragAmount = useHorizontalDrag ? deltaX : deltaY;
        float adjustedDragAmount = dragAmount * sensitivity / 100f;

        Value = Mathf.Clamp(Value + adjustedDragAmount, 0f, 1f);
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        float rotation = Mathf.Lerp(MinRotation, MaxRotation, Value);
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }
    public void OnDragBegin() { }
    public void OnDragEnd() { }

    public void OnHoverEnter() { }
    public void OnHoverExit() { }

    public void Interact()
    {

    }

    public void RightClick()
    {

    }

    public bool IsInteractable()
    {
        return isInteractable;
    }
}
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DraggingManager : MonoBehaviour
{
    private IDraggable currentDraggable;
    private IDial currentDial;
    private bool isDragging = false;
    private Vector2 lastMousePosition;
    [SerializeField] private float rotScale = 0.15f;

    [SerializeField] float clickDuration = 2;
    private bool clicking = false;
    private float totalDownTime;

    [SerializeField] private float precisionFactor = 0.5f; // apply when shift is held

    private void Start()
    {

    }

    void Update()
    {
        DetectDragOrDial();
    }

    void DetectDragOrDial()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                IDraggable draggable = hit.collider.GetComponent<IDraggable>();
                if (draggable != null && draggable.isDraggable())
                {
                    currentDraggable = draggable;
                    currentDial = draggable as IDial;

                    if (currentDraggable is MonoBehaviour draggableObject)
                    {
                        draggableObject.GetComponent<Collider>().enabled = false;
                    }

                    // Start dragging
                    currentDraggable.OnDragBegin();
                    isDragging = true;
                    lastMousePosition = Input.mousePosition;
                }
            }
        }

        // Handle dragging if currently dragging
        if (isDragging && currentDraggable != null)
        {
            if (currentDial != null)
            {
                HandleDialRotation();
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    totalDownTime = 0;
                    clicking = true;
                }

                if (clicking)
                {
                    totalDownTime += Time.deltaTime;

                    // If the long press duration is met, start dragging
                    if (totalDownTime >= clickDuration)
                    {
                        clicking = false;
                        DragObject();
                    }
                }

                // If already dragging, update position
                if (Input.GetMouseButton(0) && totalDownTime >= clickDuration)
                {
                    DragObject();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    clicking = false;
                    totalDownTime = 0;
                    HandleDragEnd();
                }
            }
        }
    }



    void HandleDialRotation()
    {
        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 delta = currentMousePosition - lastMousePosition;

        // precision factor if shift is held
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            delta *= precisionFactor;
        }

        currentDial.OnDialDrag(delta.x, delta.y);
        lastMousePosition = currentMousePosition;
    }

    void DragObject()
    {
        Vector3 mousePos = new Vector3 {  };
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                mousePos = hit.point;
            }
            else
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        print(mousePos);
        

        if (currentDraggable is MonoBehaviour draggableObject)
        {
            Vector3 currentPos = draggableObject.transform.position;
            Vector3 newPos = mousePos;

            // precision factor if shift is held
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                newPos = Vector3.Lerp(currentPos, newPos, precisionFactor); // doest actly work :(
            }

            draggableObject.transform.position = newPos;
            if (Input.mouseScrollDelta.y != 0)
            {
                draggableObject.transform.Rotate(Vector3.up * Input.mouseScrollDelta.y * rotScale);
            }
        }
    }

    void HandleDragEnd()
    {
        if (currentDial == null) // handle drops for non-dial objects
        {
            HandleDrop();
        }

        if (currentDraggable is MonoBehaviour draggableObject)
        {
            draggableObject.GetComponent<Collider>().enabled = true;
        }
        currentDraggable.OnDragEnd();
        currentDraggable = null;
        currentDial = null;
        isDragging = false;
    }

    void HandleDrop()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                Debug.Log("Dropped on " + hit.collider.gameObject);
                IReceptacle receptacle = hit.collider.GetComponent<IReceptacle>();
                if (receptacle != null && receptacle.CanAccept(currentDraggable))
                {
                    receptacle.OnItemRecieve(currentDraggable);
                    Debug.Log("Item dropped on valid receptacle.");
                }
                else
                {
                    Debug.Log("Item cannot be dropped here.");
                }
            }
        }
    }
}
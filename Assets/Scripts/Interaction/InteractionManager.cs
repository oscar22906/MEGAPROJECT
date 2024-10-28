using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private IInteractable currentInteractable;

    void Update()
    {
        DetectHover();
        DetectLeftClick();
        DetectRightClick();
    }

    void DetectHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // hover actions
                if (currentInteractable != interactable)
                {
                    // Hovering over a new object
                    currentInteractable = interactable;
                    OnHoverStart(interactable);
                }
            }
            else if (currentInteractable != null)
            {
                // No longer hovering
                OnHoverEnd(currentInteractable);
                currentInteractable = null;
            }
        }
        else if (currentInteractable != null)
        {
            // No object under the mouse
            OnHoverEnd(currentInteractable);
            currentInteractable = null;
        }
    }

    void DetectLeftClick()
    {
        if (Input.GetMouseButtonUp(0) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
    void DetectRightClick()
    {
        if (Input.GetMouseButtonUp(1) && currentInteractable != null)
        {
            currentInteractable.RightClick();
        }
    }

    void OnHoverStart(IInteractable interactable)
    {
        interactable.OnHoverEnter();
    }

    void OnHoverEnd(IInteractable interactable)
    {
        interactable.OnHoverExit();
    }
}

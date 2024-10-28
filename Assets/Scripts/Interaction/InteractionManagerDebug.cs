using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManagerDebug : MonoBehaviour
{
    private IInteractable currentInteractable;
    private RaycastHit2D currentHit; // Store 2D hit info for drawing gizmos
    public bool isHovering = false;

    void Update()
    {
        DetectHover();
        DetectClick();
    }

    void DetectHover()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convert mouse position to world position
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // Raycast in 2D with zero direction for point detection
        isHovering = false;

        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            Debug.Log(interactable);
            currentHit = hit; // Store hit info for debugging gizmos

            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    // Hovering over a new object
                    currentInteractable = interactable;
                    OnHoverStart(interactable);
                }

                isHovering = true;
            }
            else if (currentInteractable != null)
            {
                // No longer hovering over any interactable
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

    void DetectClick()
    {
        if (Input.GetMouseButtonDown(0) && currentInteractable != null)
        {
            currentInteractable.Interact();
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

    // Draw gizmos to visualize hover and click detection
    void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        // Draw a point at the mouse position in the Scene view
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(mousePosition, 0.1f); // Visualize the mouse position in world coordinates

        if (isHovering)
        {
            // Change color when hovering over something
            Gizmos.color = Color.red;

            // Draw a sphere at the hit point
            Gizmos.DrawSphere(currentHit.point, 0.2f);

            // Optionally, draw a line to the hovered object
            Gizmos.DrawLine(mousePosition, currentHit.point);
        }
    }
}

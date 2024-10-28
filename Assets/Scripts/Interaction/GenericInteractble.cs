using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteractble : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isInteractable;
    [SerializeField] private Color outlineColor;
    public int cameraIndex;
    private Renderer objectRenderer;
    private CameraManager cameraManager;

    private ICameraPosition lastPos;
    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        objectRenderer = GetComponent<Renderer>();
    }

    public void Interact()
    {
        if (IsInteractable())
        {
            MoveTo();
            Debug.Log("Interacted");
        }
    }
    public void RightClick()
    {
        if (IsInteractable())
        {
            MoveBack();
            Debug.Log("Right Clicked");
        }
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public void OnHoverEnter()
    {
        Debug.Log("Started Hovering");
        ChangeOutlineColor(outlineColor, 0.5f);
    }

    public void OnHoverExit()
    {
        Debug.Log("Stopped Hovering");
        ChangeOutlineColor(Color.black, 0.5f);
    }


    public void ChangeOutlineColor(Color newColor, float alpha)
    {
        if (objectRenderer != null)
        {
            foreach (Material material in objectRenderer.materials)
            {
                Color outlineColor = newColor;
                outlineColor.a = alpha;
                material.SetColor("_OutlineColor", outlineColor);
            }
        }
    }

    void MoveTo()
    {
        lastPos = cameraManager.currentCameraPosition;
        cameraManager.MoveTo(cameraIndex);
    }
    void MoveBack()
    {
        if (lastPos != null)
        {
            cameraManager.MoveTo(lastPos);
            lastPos = null;
        }
        else
        {
            cameraManager.MoveToPreviousCamera();
        }
    }
}

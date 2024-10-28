using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class GenericDraggable : MonoBehaviour, IDraggable
{
    private Renderer objectRenderer;
    [SerializeField] private bool canDrag;
    [SerializeField] private Color outlineColor;
    [SerializeField] private bool isInteractable;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    private Vector3 origin;

    private ICameraPosition lastPos;
    private CameraManager cameraManager;
    [SerializeField] private int cameraIndex;
    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        origin = gameObject.transform.position;
        audioSource = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<AudioSource>();
        objectRenderer = GetComponentInChildren<Renderer>();
    }
    public bool isDraggable()
    {
        return canDrag;
    }

    public void OnDragBegin()
    {
        Debug.Log("Started Dragging");
    }

    public void OnDragEnd()
    {
        Debug.Log("Stopped Dragging");
    }

    public void OnHoverEnter()
    {
        ChangeOutlineColor(outlineColor, 0.5f);
    }

    public void OnHoverExit()
    {
        ChangeOutlineColor(Color.black, 0.5f);
    }

    public void Interact()
    {
        PlaySound(clips[Random.Range(0, clips.Length)]);
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

    void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    void PlaySound(AudioClip clip, int volumeScale)
    {
        audioSource.PlayOneShot(clip, volumeScale);
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

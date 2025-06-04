using UnityEngine;

public class Monitor1Interaction : MonoBehaviour, IInteractable
{
    [SerializeField] bool isInteractable = true;
    [SerializeField] MeshCollider[] m_meshes;
    [SerializeField] Animator Animator;
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
        if (isInteractable)
        {
            if (!Animator.GetBool("ToggleON"))
            {
                Animator.SetBool("ToggleON", true);
                MoveTo();
            }
            else
                Animator.SetBool("ToggleON", false);
            Debug.Log("Interacted");
        }
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public void OnHoverEnter()
    {
        Animator.ResetTrigger("HoverExit");
        Animator.SetTrigger("HoverEnter");
    }

    public void OnHoverExit()
    {
        Animator.ResetTrigger("HoverEnter");
        Animator.SetTrigger("HoverExit");
    }

    public void RightClick()
    {
        if (IsInteractable())
        {
            if (Animator.GetBool("ToggleON"))
            {
                Animator.SetBool("ToggleON", false);
                MoveBack();
            }
            else
            Debug.Log("Right Clicked");
        }
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

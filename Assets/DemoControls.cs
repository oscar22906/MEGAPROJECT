using UnityEngine;

public class DemoControls : MonoBehaviour
{
    CameraManager cameraManager;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraManager.MoveToNextCamera();
        }
    }
}

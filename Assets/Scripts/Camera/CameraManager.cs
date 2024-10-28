using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using VInspector;

public class CameraManager : MonoBehaviour
{
    private SortedDictionary<int, ICameraPosition> sceneCameras = new SortedDictionary<int, ICameraPosition>();
    private Camera mainCamera;
    public ICameraPosition currentCameraPosition;

    [SerializeField] private float transitionDuration = 2f;

    [System.Diagnostics.Conditional("ENABLE_LOGS")] // Add ENABLE_LOGS to scripting define symbols to enable debug
    private static void Log(string message) => Debug.Log(message);

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene.");
            return;
        }

        InitializeSceneCameras();
        MoveToOrigin();
    }

    private void InitializeSceneCameras()
    {
        ICameraPosition[] cameraPositions = FindObjectsOfType<MonoBehaviour>().OfType<ICameraPosition>().ToArray();
        Log($"Found {cameraPositions.Length} ICameraPosition implementations in the scene.");

        foreach (ICameraPosition cameraPosition in cameraPositions)
        {
            int index = cameraPosition.GetIndex();
            if (sceneCameras.ContainsKey(index))
            {
                Debug.LogWarning($"Duplicate camera index: {index}. Overwriting previous entry.");
            }
            sceneCameras[index] = cameraPosition;
            Log($"Added camera position with index {index}, IsOrigin: {cameraPosition.IsOrigin()}, GameObject: {((MonoBehaviour)cameraPosition).gameObject.name}");
        }

        Log($"Total camera positions in dictionary: {sceneCameras.Count}");
    }

    [Button]
    public void MoveToOrigin()
    {
        if (sceneCameras.Count == 0)
        {
            Debug.LogError("No camera positions found in the scene. Make sure you have GameObjects with ICameraPosition implementations.");
            return;
        }

        ICameraPosition originPosition = sceneCameras.Values.FirstOrDefault(c => c.IsOrigin());
        if (originPosition != null)
        {
            Log($"Moving to origin camera at index {originPosition.GetIndex()}");
            MoveTo(originPosition.GetIndex());
        }
        else
        {
            Debug.LogWarning("No origin camera position found. Moving to the first available position.");
            MoveTo(sceneCameras.Keys.First());
        }
    }

    [Button]
    public void MoveToNextCamera()
    {
        if (sceneCameras.Count == 0)
        {
            Debug.LogError("No camera positions available.");
            return;
        }

        if (currentCameraPosition == null)
        {
            Log("No current camera position set. Moving to origin.");
            MoveToOrigin();
            return;
        }

        int currentIndex = currentCameraPosition.GetIndex();
        int nextIndex = sceneCameras.Keys.FirstOrDefault(k => k > currentIndex);
        if (nextIndex == 0) // If no larger index found, wrap around to the smallest
        {
            nextIndex = sceneCameras.Keys.First();
        }
        Log($"Moving to next camera at index {nextIndex}");
        MoveTo(nextIndex);
    }

    [Button]
    public void MoveToPreviousCamera()
    {
        if (sceneCameras.Count == 0)
        {
            Debug.LogError("No camera positions available.");
            return;
        }

        if (currentCameraPosition == null)
        {
            Log("No current camera position set. Moving to origin.");
            MoveToOrigin();
            return;
        }

        int currentIndex = currentCameraPosition.GetIndex();
        int previousIndex = sceneCameras.Keys.LastOrDefault(k => k < currentIndex);
        if (previousIndex == 0) // If no smaller index found, wrap around to the largest
        {
            previousIndex = sceneCameras.Keys.Last();
        }
        Log($"Moving to previous camera at index {previousIndex}");
        MoveTo(previousIndex);
    }

    [Button]
    public void MoveTo(int index)
    {
        if (!sceneCameras.TryGetValue(index, out ICameraPosition targetCameraPosition))
        {
            Debug.LogWarning($"No camera found with index: {index}");
            return;
        }

        if (targetCameraPosition is MonoBehaviour monoBehaviour)
        {
            mainCamera.transform.DOMove(monoBehaviour.transform.position, transitionDuration);
            mainCamera.transform.DORotate(monoBehaviour.transform.rotation.eulerAngles, transitionDuration);
            currentCameraPosition = targetCameraPosition;
            Log($"Moved to camera at index {index}, GameObject: {monoBehaviour.gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Camera position with index {index} is not a MonoBehaviour.");
        }
    }
    public void MoveTo(ICameraPosition targetCameraPosition)
    {

        if (targetCameraPosition is MonoBehaviour monoBehaviour)
        {
            mainCamera.transform.DOMove(monoBehaviour.transform.position, transitionDuration);
            mainCamera.transform.DORotate(monoBehaviour.transform.rotation.eulerAngles, transitionDuration);
            currentCameraPosition = targetCameraPosition;
            Log($"Moved to camera, GameObject: {monoBehaviour.gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Camera position is not a MonoBehaviour.");
        }
    }

    [Button]
    public void LogCameraInfo()
    {
        Debug.Log($"Total camera positions: {sceneCameras.Count}");
        foreach (var kvp in sceneCameras)
        {
            MonoBehaviour mb = (MonoBehaviour)kvp.Value;
            Debug.Log($"Camera Index: {kvp.Key}, Is Origin: {kvp.Value.IsOrigin()}, GameObject: {mb.gameObject.name}, Position: {mb.transform.position}");
        }
    }
}
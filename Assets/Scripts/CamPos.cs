using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Position", menuName = "CamPos/Create New Camera Position")]
public class CamPos : ScriptableObject
{
    public int order;
    public string sceneName;
    public bool isOrigin;
    public Transform pos;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCamera : MonoBehaviour, ICameraPosition
{
    public int index;
    public bool isOrigin = false;

    public bool IsOrigin()
    {
        return isOrigin;
    }
    public int GetIndex()
    {
        return index;
    }
}

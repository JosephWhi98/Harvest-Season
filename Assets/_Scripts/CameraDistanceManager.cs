using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraDistanceManager : MonoBehaviour
{
    public float[] distances;

    [Button]
    public void GetDistances()
    {
        distances = GetComponent<Camera>().layerCullDistances;
    }

    public void Awake() 
    {
        GetComponent<Camera>().layerCullDistances = distances;
    }
}

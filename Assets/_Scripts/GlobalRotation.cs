using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GlobalRotation : MonoBehaviour
{
    public float rotationSpeed = 10.0f;
    public Vector3 rotationAxis = Vector3.up;

    // Update is called once per frame
    void Update()
    {
     
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
        
    }
}

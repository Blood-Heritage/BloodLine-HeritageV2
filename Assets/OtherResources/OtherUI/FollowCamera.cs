using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 locationOffset;
    
    public void AssignTarget(Transform _target)
    {
        target = _target;
    }
    
    // This script is made for the MiniMap camera
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + locationOffset;
        transform.position = desiredPosition;
        
        Vector3 targetEuler = target.rotation.eulerAngles;
        
        // Queremos que X sea 90, y que Y y Z sigan al personaje
        Vector3 cameraEuler = new Vector3(90f, targetEuler.y, targetEuler.z);
    }
}

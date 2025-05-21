using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 locationOffset;
    public float rotationYOffset = 1f;
    public bool trackRotation = true;
    
    public float X;
    public float Y;
    public float Z;
    public float W;
    
    public float custom_X;
    public float custom_Y;
    public float custom_Z;
    public float custom_W;
    
    public bool block_X = false;
    public bool block_Y = false;
    public bool block_Z = false;
    public bool block_W = false;
    
    

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

        /*
        transform.rotation = Quaternion.Euler(cameraEuler);
        
        X = target.rotation.x;
        Y = target.rotation.y;
        Z = target.rotation.z;
        W = target.rotation.w;
        
        if (!trackRotation)
        {
            var rotation = target.rotation;

            if (block_X)
                rotation.x = custom_X;
            
            if (block_Y)
                rotation.y = custom_Y;
            
            if (block_Z)
                rotation.z = custom_Z;
            
            if (block_W)
                rotation.w = custom_W;
            
            transform.rotation = rotation;        
        }
        else
        {
            // transform.rotation = target.rotation;

            var rotation = transform.rotation;
            rotation.x = 1f;
            rotation.y = target.rotation.y;
            rotation.z = target.rotation.z;
            rotation.w = 1f;
        
            transform.rotation = rotation;
            
            // transform.rotation = new Quaternion(.9f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }
        */
    }
}

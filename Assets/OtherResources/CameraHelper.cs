using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    [System.Serializable]
    public struct CameraHelpers
    {
        public Transform orientation;
        public Transform follow;
    }
    
    public Transform orientation;
    public Transform follow;
    

    // Start is called before the first frame update
    public CameraHelpers GetHelpers()
    {
        return new CameraHelpers()
        {
            orientation = orientation,
            follow = follow,
        };
    }
}

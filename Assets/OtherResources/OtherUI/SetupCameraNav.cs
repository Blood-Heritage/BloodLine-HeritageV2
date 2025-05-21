using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCameraNav : MonoBehaviour
{
    private Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void SetupCamera(Camera camera)
    {
        canvas.worldCamera = camera;   
    }
}

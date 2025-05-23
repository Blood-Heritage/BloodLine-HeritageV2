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

    private void Start()
    {
        canvas.worldCamera = Minimap.Instance.camera;
    }
}

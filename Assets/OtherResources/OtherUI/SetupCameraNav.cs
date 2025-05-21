using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCameraNav : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private GameObject enemyNav;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void SetupCamera(Camera camera)
    {
        enemyNav.SetActive(false);
        canvas.worldCamera = camera;   
    }
}

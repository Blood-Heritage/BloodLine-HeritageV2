using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private GameObject minimap;
    [SerializeField] private GameObject bigmap;
    
    public static Minimap Instance;
    public Camera camera;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            BARManager.Instance.minimap = this;
            camera = GetComponentInChildren<Camera>();
        }
        else
        {
            Debug.LogError("Minimap already exists!");
            Destroy(gameObject);
        }
    }


    public void Toggle()
    {
        minimap.SetActive(!minimap.activeSelf);
        bigmap.SetActive(!bigmap.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PanelType
{
    None,
    Main,
    Settings,
    Credits
}

public class MENUCONTROLLER : MonoBehaviour
{
    private GameManager manager;

    private void Start()
    {
        manager = GameManager.instance;
    }

    public void OpenPanel()
    {

    }

    public void ChangeScene(string _scenename)
    {
        manager.ChangeScene(_scenename);
    }

    public void Quit()
    {
        manager.Quit();
    }
    
}

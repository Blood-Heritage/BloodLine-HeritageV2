using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MENUCONTROLLER : MonoBehaviour
{
    public void ChangeScene(string _scenename)
    {
        SceneManager.LoadScene(_scenename);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

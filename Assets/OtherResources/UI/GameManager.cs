using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance { private set; get;}

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeScene(string _scenename)
    {
        SceneManager.LoadScene(_scenename);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

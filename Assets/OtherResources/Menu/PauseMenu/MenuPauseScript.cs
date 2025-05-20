using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MenuPauseScript : MonoBehaviour
{
    public GameObject pauseMenuUI; // Ton UI de pause
    public MonoBehaviour cameraScript; // Ton script de mouvement de caméra
    
    public Text pingText; 

    private bool isPaused = false;
    
    
    void Update()
    {
            int ping = PhotonNetwork.GetPing();
            pingText.text = $"Ping: {ping} ms";
            if (ping < 100)
                pingText.color = Color.green;
            else if (ping < 200)
                pingText.color = Color.yellow;
            else
                pingText.color = Color.red;
                
    }

}
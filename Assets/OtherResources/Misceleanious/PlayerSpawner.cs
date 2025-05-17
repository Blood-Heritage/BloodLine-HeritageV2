using System;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public Vector3 spawnPosition = new Vector3(80, 0, 56); // Position initiale du spawn
    public int vcPriority = 20;
    public GameObject playerPrefab; // Prefab du personnage
    public GameObject camerasPrefab;
    void Start()
    {
        // Verifie que le prefab est assigne
        if (playerPrefab != null)
        {
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned in the inspector!");
                return;
            }
            // Instancie le personnage pour ce joueur
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);


            PhotonView view = player.GetComponent<PhotonView>();
            if (view.IsMine)
            {
                // crea cameras
                GameObject holder = Instantiate(camerasPrefab);
                
                var wrapperScript = holder.GetComponent<CameraHolder_GameScene>();
                if (wrapperScript == null)
                {
                    Debug.LogError("WrapperCharacterCamera script could not be found");
                    throw new Exception("No wrapperScript was found");
                }
                else
                {
                    wrapperScript.SetupCameras(player);
                    Debug.Log("it seems that the wrapper was found");
                }
            }
        }
    }
}

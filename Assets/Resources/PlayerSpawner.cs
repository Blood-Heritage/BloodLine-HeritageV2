using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Prefab du personnage
    public Vector3 spawnPosition = new Vector3(80, 0, 56); // Position initiale du spawn
    public int vcPriority = 20;
    
    
    void Start()
    {
        // Verifie que le prefab est assigne
        if (playerPrefab != null)
        {
            // Instancie le personnage pour ce joueur
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned in the inspector!");
            }
            
            player.BroadcastMessage("ChangeCameraPriority", vcPriority);
            vcPriority--;

            // player.GetComponent<CinemachineVirtualCamera>().Priority;

            if (Camera.main == null)
            {
                Debug.LogError("Main Camera is missing in the scene!");
            }

            // Si une camera doit suivre ce personnage, on la configure ici
            if (Camera.main != null && Camera.main.GetComponent<CameraController>() != null)
            {
                Camera.main.GetComponent<CameraController>().target = player.transform;
            }


        }
    }
}

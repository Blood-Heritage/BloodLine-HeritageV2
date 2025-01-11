using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab du personnage
    public Vector3 spawnPosition = new Vector3(80, 0, 56); // Position initiale du spawn

    void Start()
    {
        // Vérifie que le prefab est assigné
        if (playerPrefab != null)
        {
            // Instancie le personnage pour ce joueur
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned in the inspector!");
            }

            if (Camera.main == null)
            {
                Debug.LogError("Main Camera is missing in the scene!");
            }

            // Si une caméra doit suivre ce personnage, on la configure ici
            if (Camera.main != null && Camera.main.GetComponent<CameraController>() != null)
            {
                Camera.main.GetComponent<CameraController>().target = player.transform;
            }


        }
    }
}

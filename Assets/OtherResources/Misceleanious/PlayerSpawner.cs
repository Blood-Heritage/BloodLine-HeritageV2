using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public Vector3 spawnPosition = new Vector3(80, 0, 56); // Position initiale du spawn
    public int vcPriority = 20;
    public GameObject playerPrefab; // Prefab du personnage
    public GameObject CameraPrefab;
    public GameObject wrapperPlayerCameraMulti;
    void Start()
    {
        // Verifie que le prefab est assigne
        if (playerPrefab != null)
        {
            var wrapper = Instantiate(wrapperPlayerCameraMulti);
            var camaras = Instantiate(CameraPrefab, wrapper.transform);
            
            // Instancie le personnage pour ce joueur
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned in the inspector!");
                return;
            }

            
            // player.transform.SetParent(wrapper.transform);

            wrapper.GetComponent<WrapperCharacterCamera>().SetupCameras(camaras, player , player.transform);
        }
    }
}

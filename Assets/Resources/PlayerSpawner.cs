using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Prefab du personnage
    public Vector3 spawnPosition = new Vector3(80, 0, 56); // Position initiale du spawn
    public int vcPriority = 20;

    public GameObject CameraPrefab;
    
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

            var cinemachinecamera = Instantiate(CameraPrefab);
            
            cinemachinecamera.BroadcastMessage("ChangeCameraPriority", vcPriority);
            vcPriority--;
            
            // Transform _orientation, Transform _player, Transform _playerObj
            
            // get Helper
            CameraHelper cameraHelper = player.GetComponentInChildren<CameraHelper>();
            var helpers = cameraHelper.GetHelpers();
            
            Debug.Log($"orientation: {helpers.orientation}, follow: {helpers.follow}");
            
            
            // get ThirPerson script to set the variables
            ThirdPersonCam thirdPersonCamScript = cinemachinecamera.GetComponent<ThirdPersonCam>();
            if (thirdPersonCamScript == null)
            {
                Debug.LogError("Third Person Camera Script Is NULL");
            }
            
            
            thirdPersonCamScript.SetVariablesCustom(helpers.orientation, helpers.follow, player.transform);
            

            if (Camera.main == null)
            {
                Debug.LogError("Main Camera is missing in the scene!");
            }

            // Si une camera doit suivre ce personnage, on la configure ici
            if (Camera.main != null && Camera.main.GetComponent<CameraController>() != null)
            {
                Camera.main.GetComponent<CameraController>().target = player.transform;
            }
            else
            {
                Debug.LogError("No deberia llegar a aca!!!");
            }
        }
    }
}

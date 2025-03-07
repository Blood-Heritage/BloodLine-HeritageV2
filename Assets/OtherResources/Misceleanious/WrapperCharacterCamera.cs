
using System;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class WrapperCharacterCamera : MonoBehaviour
{

    public GameObject character;
    public GameObject cameraHolder;
    
    private static bool IsOnline()
    {
        return PhotonNetwork.IsConnected && PhotonNetwork.InRoom;
    }

    private void Start()
    {
        if (!IsOnline())
        {
            if (character == null || cameraHolder == null)
            {
                Debug.LogError("character object or cameraholder is null");
                return;
            } 
            
            SetupCameras(character, cameraHolder);
        }
    }
    
    
    
    public void SetupCameras(GameObject CameraHolder, GameObject character, Transform follow)
    {
        var charac = character.GetComponent<MovementReborn>();
        // var follow = character.gameObject.Find("follow");

        // GameObject follow = character.transform.Find("follow")?.gameObject;

        if (follow != null) Debug.Log($"Objeto 'follow' encontrado: {follow.name}");
        else
        {
            Debug.LogError(" No se encontró un objeto llamado 'follow' en los hijos directos.");
            return;
        }
        
        var cams = CameraHolder.GetComponentsInChildren<ICinemachineCamera>();
        if (cams.Length == 0)
        {
            Debug.LogError("No cameras were found in CameraHolder. !!!!");
        }
        
        ICinemachineCamera? normal = null;
        ICinemachineCamera? aim = null;

        if (cams.Length < 2)
        {
            Debug.LogError($"Found {cams.Length} cinemachine cameras");
            return;
        }
        
        
        foreach (var cam in cams)
        {
            switch (cam.Priority)
            {
                case 20:
                    normal = cam;
                    break;
                case 30:
                    aim = cam;
                    cam.VirtualCameraGameObject.gameObject.SetActive(false);
                    break;
            }
            
            // cam.LookAt = follow;
            cam.Follow = follow;
        }
        
        
        charac.SetCameras(normal!, aim!, normal!.VirtualCameraGameObject.transform);
    
    }


    public void SetupCameras(GameObject character, GameObject CameraHolder)
    {
        var charac = character.GetComponent<MovementReborn>();
        // var follow = character.gameObject.Find("follow");

        GameObject follow = character.transform.Find("follow")?.gameObject;

        if (follow != null) Debug.Log($"Objeto 'follow' encontrado: {follow.name}");
        else
        {
            Debug.LogError(" No se encontró un objeto llamado 'follow' en los hijos directos.");
            return;
        }
        
        var cams = CameraHolder.GetComponentsInChildren<ICinemachineCamera>();
        if (cams.Length == 0)
        {
            Debug.LogError("No cameras were found in CameraHolder. !!!!");
        }
        
        ICinemachineCamera? normal = null;
        ICinemachineCamera? aim = null;

        if (cams.Length < 2)
        {
            Debug.LogError($"Found {cams.Length} cinemachine cameras");
            return;
        }
        
        
        foreach (var cam in cams)
        {
            switch (cam.Priority)
            {
                case 20:
                    normal = cam;
                    break;
                case 30:
                    aim = cam;
                    cam.VirtualCameraGameObject.gameObject.SetActive(false);
                    break;
            }
            
            // cam.LookAt = follow;
            cam.Follow = follow.transform;
        }
        
        
        charac.SetCameras(normal!, aim!, normal!.VirtualCameraGameObject.transform);
    
    }
}
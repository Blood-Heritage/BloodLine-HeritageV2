
using System;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class WrapperCharacterCamera : MonoBehaviour
{

    public GameObject Character;
    public GameObject cameraHolder;
    
    private static bool IsOnline()
    {
        return PhotonNetwork.IsConnected && PhotonNetwork.InRoom;
    }

    private void Start()
    {
        // for testing
        if (!IsOnline())
        {
            if (Character == null || cameraHolder == null)
            {
                Debug.LogError("character object or cameraholder is null");
                return;
            } 
            
            SetupCameras(Character, cameraHolder);
        }
    }


    public void SetupCameras(GameObject cameras)
    {
        var character = GetComponentInChildren<MovementReborn>();
        if (character.CinemachineCameraTarget != null) Debug.Log($"Objeto 'follow' encontrado");
        else
        {
            Debug.LogError(" No se encontró un objeto llamado 'follow' en los hijos directos.");
            return;
        }
                
        var cams = cameras.GetComponentsInChildren<ICinemachineCamera>();
        if (cams.Length == 0)
        {
            Debug.LogError("No cameras were found in CameraHolder. !!!!");
            throw new Exception("No cameras were found in CameraHolder.");
        }
                

        if (cams.Length < 2)
        {
            Debug.LogError($"Found {cams.Length} cinemachine cameras");
            throw new Exception("Found less cameras than expected.");
            return;
        }
        else if (cams.Length > 2)
        {
            Debug.LogError($"Found {cams.Length} cinemachine cameras");
            throw new Exception("Found more cameras than expected.");
            return;
        }
        
        ICinemachineCamera? normal = null;
        ICinemachineCamera? aim = null;
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
            cam.Follow = character.CinemachineCameraTarget.transform;
        }
        
        Debug.Log($"normal: {normal}, aim: {aim}");
        Debug.Log($"Are they null?? -> normal: {normal==null}, aim: {aim==null}");
        
        character.SetCameras(normal!, aim!, normal!.VirtualCameraGameObject.transform);
    }
    
    public void SetupCameras(GameObject character, Transform follow)
    {
        Character = character;
        var charac = character.GetComponent<MovementReborn>();
        // var follow = character.gameObject.Find("follow");

        // GameObject follow = character.transform.Find("follow")?.gameObject;

        if (follow != null) Debug.Log($"Objeto 'follow' encontrado: {follow.name}");
        else
        {
            Debug.LogError(" No se encontró un objeto llamado 'follow' en los hijos directos.");
            return;
        }
        
        var cams = GetComponentsInChildren<ICinemachineCamera>();
        if (cams.Length == 0)
        {
            Debug.LogError("No cameras were found in CameraHolder. !!!!");
            
            throw new Exception("No cameras were found in CameraHolder.");
        }
        
        ICinemachineCamera? normal = null;
        ICinemachineCamera? aim = null;

        if (cams.Length < 2)
        {
            Debug.LogError($"Found {cams.Length} cinemachine cameras");
            throw new Exception("Found more cameras than expected.");
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

        if (cams.Length < 2 || cams.Length > 2)
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
                    break;
            }
            
            // cam.LookAt = follow;
            cam.Follow = follow.transform;
        }
        
        
        charac.SetCameras(normal!, aim!, normal!.VirtualCameraGameObject.transform);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHolder_GameScene : MonoBehaviour
{
    public void SetupCameras(GameObject character)
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
        
        var cams = GetComponentsInChildren<ICinemachineCamera>();
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

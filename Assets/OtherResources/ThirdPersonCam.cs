using System;
using Cinemachine;
using UnityEngine;
using Photon.Pun;

public class ThirdPersonCam : MonoBehaviourPun
{
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public CinemachineVirtualCamera VirtualCamera;
    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetCameraObjectCustom(CinemachineVirtualCamera cameraObject)
    {
        VirtualCamera = cameraObject;
    }

    private void Update()
    {
        if (orientation == null || player == null || playerObj == null ||  VirtualCamera == null)
        {
            Debug.Log("orientation | player | playerObj | VirtualCamera in thirdPersonCam null");
            return;
        }    
        
        // Verifie si le joueur controle ce personnage
        if (!photonView.IsMine)
        {
            return; // Ignore les mouvements des autres joueurs
        }
        
        Vector3 viewDir = player.position - new Vector3(VirtualCamera.transform.position.x, player.position.y, VirtualCamera.transform.position.z);
        orientation.forward = viewDir.normalized;
        
        // rotate player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 inputDir = orientation.forward * vertical + orientation.right * horizontal;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, rotationSpeed * Time.deltaTime);
        }
    }


}
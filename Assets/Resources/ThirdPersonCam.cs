using System;
using UnityEngine;
using Photon.Pun;

public class ThirdPersonCam : MonoBehaviourPun
{
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    
    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetVariablesCustom(Transform _orientation, Transform _player, Transform _playerObj)
    {
        orientation = _orientation;
        player = _player;
        playerObj = _playerObj;
    }

    private void Update()
    {
        if (orientation == null || player == null || playerObj == null)
        {
            Debug.Log("orientation | player | playerObj in thirdPerson camera null");
            return;
        }    
        
        // Verifie si le joueur controle ce personnage
        if (!photonView.IsMine)
        {
            return; // Ignore les mouvements des autres joueurs
        }
        
        Vector3 viewDir = new Vector3(player.position.x, 0, player.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        orientation.forward = viewDir.normalized;
        
        // rotate player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 inputDir = orientation.forward * vertical + orientation.right * horizontal;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir, rotationSpeed * Time.deltaTime);
        }
    }


}
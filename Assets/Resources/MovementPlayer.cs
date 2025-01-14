using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    public float runningSpeed = 5f;
    public float walinkingspeed = 2.5f;
    public float rotationSpeed = 725f;
    public KeyCode forRunning = KeyCode.LeftShift;
    public CinemachineVirtualCamera vc;
    
    void Update()
    {
        // Verifie si le joueur controle ce personnage
        if (!photonView.IsMine)
        {
            return; // Ignore les mouvements des autres joueurs
        }

        vc.Priority = 1;
    
        // Gestion des mouvements (clavier : WASD ou fleches)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            
            bool runningKeyPressed = Input.GetKey(forRunning);
            if (runningKeyPressed)
            {
                transform.Translate(direction * runningSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Translate(direction * walinkingspeed * Time.deltaTime, Space.World);
            }
        }
    }
}
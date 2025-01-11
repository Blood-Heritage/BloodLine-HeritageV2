using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    Animator animator;
    public float runningSpeed = 5f;
    public float walinkingspeed = 2.5f;
    public KeyCode forRunning = KeyCode.LeftShift;

    int isWalkingHash;
    int isRunningHash;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        // more performant
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }
    
    
    
    void Update()
    {
        // Verifie si le joueur controle ce personnage
        if (!photonView.IsMine)
        {
            return; // Ignore les mouvements des autres joueurs
        }

        // Gestion des mouvements (clavier : WASD ou fleches)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        Debug.Log($"direction: {direction}");

        bool runningKeyPressed = Input.GetKey(forRunning);
        if (direction.magnitude > 0.1f)
        {
            Animation(true);
            if (runningKeyPressed)
            {
                transform.Translate(direction * runningSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Translate(direction * walinkingspeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            Animation(false);
        }
    }
}

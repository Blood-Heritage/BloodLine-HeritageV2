using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5f;

    void Update()
    {
        // Vérifie si le joueur contrôle ce personnage
        if (!photonView.IsMine)
        {
            return; // Ignore les mouvements des autres joueurs
        }

        // Gestion des mouvements (clavier : WASD ou flèches)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }
}

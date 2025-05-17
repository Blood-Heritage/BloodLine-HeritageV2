using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    public float maxHealth = 100f;
    public float health = 100f;
    
    private void Start()
    {
        if (photonView.IsMine && BARManager.Instance != null)
        {
            BARManager.Instance.AssignPlayer(photonView);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Prevents negative values
    }
}

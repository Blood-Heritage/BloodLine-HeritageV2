using UnityEngine;
using Photon.Pun;
using TMPro;

public class Health : MonoBehaviour
{
    [Header("UI")]
    public int health;
    public TextMeshProUGUI healthText;

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;
        healthText.text = health.ToString();
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

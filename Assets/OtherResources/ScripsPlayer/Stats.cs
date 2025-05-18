using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.Serialization;

public class Stats : MonoBehaviourPun
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float health = 100f;
    
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina;
    public float staminaDrainRate = 15f; // Stamina decrease per second while running
    public float staminaRegenRate = 10f; // Stamina increase per second while not running
    // public float staminaThreshold = 10f; // Minimum stamina required to run
    public float staminaCooldown = 5f;    // Time before player can run again when stamina is 0
    public bool CanRun => !isExhausted;
    private bool isExhausted = false;
    
    private MovementReborn movement;
    
    private void Awake()
    {
        movement = GetComponent<MovementReborn>();
    }
    
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
    
    private void Update()
    {
        if (!photonView.IsMine) return;
        
        // if (health)
        if (movement.isRunningPressed && !movement.isShootingPressed) // && currentStamina > staminaThreshold)
            DrainStamina();
        else
            RegenerateStamina();
    }

    public void DrainStamina()
    {
        if (isExhausted) return;
        
        // Debug.Log("Draining stamina");
        stamina -= staminaDrainRate * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        if (stamina <= 0 && !isExhausted)
        {
           StartCoroutine(ExhaustionCooldown());
        }
    }

    private IEnumerator ExhaustionCooldown()
    {
        isExhausted = true;
        // movement.isRunningPressed = false;  // Force player to walk
        // Debug.Log("Stamina Depleted! Player must walk for 5 seconds.");
        
        yield return new WaitForSeconds(staminaCooldown);
        
        isExhausted = false;
        // Debug.Log("Player can run again.");
    }

    public void RegenerateStamina()
    {
        // Debug.Log("Regenerating stamina");
        stamina += staminaRegenRate * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
    }
}

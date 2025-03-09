using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Stamina : MonoBehaviourPun
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 15f; // Stamina decrease per second while running
    public float staminaRegenRate = 10f; // Stamina increase per second while not running
    // public float staminaThreshold = 10f; // Minimum stamina required to run
    public bool isExhausted = false;    // If true, player can't run
    public float staminaCooldown = 5f;    // Time before player can run again when stamina is 0
    
    private MovementReborn movement;
    private bool isRunning;
    private int isRunningHash;
    private int isShootingHash;

    private void Start()
    {
        isRunningHash = Animator.StringToHash("isRunning");
        isShootingHash = Animator.StringToHash("isShooting");
        currentStamina = maxStamina;
        movement = GetComponent<MovementReborn>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        

        if (movement.animator.GetBool(isRunningHash) && !movement.animator.GetBool(isShootingHash)) // && currentStamina > staminaThreshold)
        {
            DrainStamina();
        }
        else
        {
            RegenerateStamina();
        }
        
        BARManager.Instance.UpdateStaminaBar(currentStamina, maxStamina);
        // movement.Animation();
    }

    public void DrainStamina()
    {
        if (isExhausted) return;
        
        Debug.Log("Draining stamina");
        currentStamina -= staminaDrainRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (currentStamina <= 0 && !isExhausted)
        {
           StartCoroutine(ExhaustionCooldown());
        }
    }

    private IEnumerator ExhaustionCooldown()
    {
        isExhausted = true;
        movement.isRunningPressed = false;  // Force player to walk
        // Debug.Log("Stamina Depleted! Player must walk for 5 seconds.");
        
        yield return new WaitForSeconds(staminaCooldown);
        
        isExhausted = false;
        // Debug.Log("Player can run again.");
    }

    public void RegenerateStamina()
    {
        Debug.Log("Regenerating stamina");
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

}

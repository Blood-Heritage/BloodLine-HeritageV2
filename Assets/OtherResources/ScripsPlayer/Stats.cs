using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using OtherResources.Interfaces;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class Stats : IHealth
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _health = 100f;
    public new float maxHealth => _maxHealth;
    public new float health => _health;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina;
    public float staminaDrainRate = 8f; // Stamina decrease per second while running
    public float staminaRegenRate = 5f; // Stamina increase per second while not running
    // public float staminaThreshold = 10f; // Minimum stamina required to run
    public float staminaCooldown = 3f;    // Time before player can run again when stamina is 0
    public bool CanRun => !isExhausted;
    private bool isExhausted = false;
    private Func<bool> isOnline;
    
    private MovementReborn movement;
    
    private void Awake()
    {
        movement = GetComponent<MovementReborn>();
    }

    
    private void Start()
    {
        if (movement.IsOnline())
        {
            if (photonView.IsMine && BARManager.Instance != null)
            {
                BARManager.Instance.AssignPlayer(photonView);
                stamina = maxStamina;
            }
        }
    }

    [PunRPC]
    public override void TakeDamage(int damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(health, 0, maxHealth); // Prevents negative values
    }
    
    
    [PunRPC]
    public void FULL_REGENERATE()
    {
        _health = maxHealth;
        stamina = maxStamina;
    }
    
    private void Update()
    {
        if (!photonView.IsMine) return;

        if (!isExhausted && movement.isRunningPressed && !movement.isShootingPressed && !movement.GoingBackwards) // && currentStamina > staminaThreshold)
            DrainStamina();
        else
            RegenerateStamina();
    }
    
    public IEnumerator Die()
    { 
        movement.animator.SetBool("isDead", true);
        Debug.LogError("Is dead, waiting 3 seconds");
        
        // wait three seconds to delete gameobject
        yield return new WaitForSeconds(3.5f);
        Debug.LogError("waited");
     
        // look into DestroyNetwork
        DestroyOnNetwork();
    }

    public void DrainStamina()
    {
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

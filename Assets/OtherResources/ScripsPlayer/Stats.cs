using System;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using OtherResources.Interfaces;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class Stats : MonoBehaviourPun, IHealth
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _health = 100f;
    public float maxHealth => _maxHealth;
    public float health => _health;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina;
    public float staminaDrainRate = 15f; // Stamina decrease per second while running
    public float staminaRegenRate = 10f; // Stamina increase per second while not running
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
    public void TakeDamage(int damage)
    {
        Debug.Log($"Received damage, now: {health}");
        _health -= damage;
        _health = Mathf.Clamp(health, 0, maxHealth); // Prevents negative values
        Debug.Log($"after: {health}");
    }
    
    
    public void LocalTakeDamage(int damage)
    {
        Debug.Log($"Received damage, now: {health}");
        _health -= damage;
        _health = Mathf.Clamp(health, 0, maxHealth); // Prevents negative values
        Debug.Log($"after: {health}");
    }
    
    private void Update()
    {
        if (movement.IsOnline())
            if (!photonView.IsMine) return;
        
        if (!isExhausted && movement.isRunningPressed && !movement.isShootingPressed && !movement.GoingBackwards) // && currentStamina > staminaThreshold)
            DrainStamina();
        else
            RegenerateStamina();
    }
    
    public IEnumerator Die()
    { 
        movement.animator.SetBool("isDead", true);
        
        // wait three seconds to delete enemy
        yield return new WaitForSeconds(3f);
        
        // what to do
        if (movement.IsOnline())
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);   
        }
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

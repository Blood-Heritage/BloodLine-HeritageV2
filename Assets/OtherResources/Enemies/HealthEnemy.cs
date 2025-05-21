using System;
using System.Collections;
using System.Collections.Generic;
using OtherResources.Interfaces;
using Photon.Pun;
using UnityEngine;

public class HealthEnemy : MonoBehaviour, IHealth
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 20f;
    [SerializeField] private float _health = 20f;
    
    public float maxHealth => _maxHealth;
    public float health => _health;
    
    private Animator animator;
    
    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health <= 0f)
            Die();
    }


    [PunRPC]
    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(health, 0, maxHealth); // Prevents negative values
    }
    
    public IEnumerator Die()
    { 
        Debug.Log("Ohhhh nooo I died :(");
        animator.SetBool("isDead", true);
        
        // wait three seconds to delete enemy
        yield return new WaitForSeconds(3f);
        // PhotonNetwork.Destroy(gameObject);
        Destroy(gameObject);
    }
}

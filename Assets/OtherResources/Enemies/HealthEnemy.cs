using System;
using System.Collections;
using System.Collections.Generic;
using OtherResources.Interfaces;
using Photon.Pun;
using UnityEngine;

public class HealthEnemy : IHealth
{
    private static readonly int IsDead = Animator.StringToHash("isDead");

    [Header("Health")]
    [SerializeField] private float _maxHealth = 30f;
    [SerializeField] private float _health = 30f;
    
    public float maxHealth => _maxHealth;
    public float health => _health;
    
    public Animator animator;
    public EnemyAnimation enemyAnimator;
    
    public void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimation>();
        animator = enemyAnimator.animator;
    }

    private void Update()
    {
        if (health <= 0f)
        {
            var enumerator = Die();
        }
    }

    [PunRPC]
    public override void TakeDamage(int damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(health, 0, maxHealth); // Prevents negative values

        if (_health <= 0f)
            StartCoroutine(Die());
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator Die()
    { 
        Debug.Log("Ohhhh nooo I died :(");
        animator.SetBool(IsDead, true);
        
        // wait three seconds to delete enemy
        yield return new WaitForSeconds(3f);
        // PhotonNetwork.Destroy(gameObject);
        DestroyOnNetwork();
    }


    public void DieToucheTriche()
    {
        DestroyOnNetwork();
    }
}

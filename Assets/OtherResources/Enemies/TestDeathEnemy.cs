using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeathEnemy : MonoBehaviour
{
    public float deathTimer = 6f;
    private HealthEnemy healthEnemy;
    public bool isDead = false;
    
    private void Awake()
    {
        healthEnemy = GetComponent<HealthEnemy>();
    }

    public void Update()
    {
        if (isDead) return;
        
        if (deathTimer > 0)
            deathTimer -= Time.deltaTime;
        else
        {
            Debug.Log("Death");
            isDead = true;
            StartCoroutine(healthEnemy.Die());
            Debug.Log("After Death");
        }
    }
}

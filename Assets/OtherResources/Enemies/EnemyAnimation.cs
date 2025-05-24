using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviourPun
{
    private EnemyAI _enemyAI;
    public Animator animator;
    private NavMeshAgent agent;
    private bool isAttacking => _enemyAI.isAtacking;
    private bool isDead => healthComponent.health <= 0f;

    private HealthEnemy healthComponent;
    
    int isShootingHash; 
    int velocityZHash;

    int TorsoLayer;
    int ShootingLayer;

    private float velocity
    {
        get
        {
            if (_enemyAI.blockedByPlayer) return 0f;
            return NormalizeCustom(_enemyAI.Velocity_Z);
        }
    }
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
        healthComponent = GetComponent<HealthEnemy>();
        
        velocityZHash = Animator.StringToHash("Velocity Z");
        isShootingHash = Animator.StringToHash("isShooting");
        
        TorsoLayer = animator.GetLayerIndex("Torso");
        ShootingLayer = animator.GetLayerIndex("Aim");
    }

    float Remap(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
    }
    
    float NormalizeCustom(float value)
    {
        return Remap(value, 2f, 4f, 0.45f, 2.0f);
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        if (isDead) ZeroAnimate();
        else Animate();
    }

    public void ZeroAnimate()
    {
        animator.SetBool(isShootingHash, false);
        animator.SetLayerWeight(TorsoLayer, 0.0f);
        animator.SetLayerWeight(ShootingLayer, 0.0f);
        animator.SetFloat(velocityZHash, 0f);
    }

    void Animate()
    {
        bool isShooting = animator.GetBool(isShootingHash);
        
        if (isAttacking) animator.SetLayerWeight(TorsoLayer, 0.5f);
        else animator.SetLayerWeight(TorsoLayer, 0.0f);
        
        if (isAttacking)
            animator.SetLayerWeight(ShootingLayer, 1.0f);
                
        if (!isAttacking)
            animator.SetLayerWeight(ShootingLayer, 0.0f);
                
                
        if (_enemyAI.isAtacking && !isShooting)
        {
            // animator.SetLayerWeight(ShootingLayer, 1.0f);
            animator.SetBool(isShootingHash, true);
        }
        
        if (!_enemyAI.isAtacking && isShooting)
        {
            // animator.SetLayerWeight(ShootingLayer, 0.0f);
            animator.SetBool(isShootingHash, false);
        }
        
        animator.SetFloat(velocityZHash, velocity);
        // animator.SetFloat(velocityZHash, Velocity_Z);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyAI : MonoBehaviourPun
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, WhatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    
    public float walkPointRange;
    public bool isAtacking;
    
    public float sightRange, attackrange;
    public bool playerInSightRange, playerInAttackRange;
    public float acceleration;
    
    public float distanceFromPlayer;
    public bool blockedByPlayer;

    public float Velocity_Z = 2f; 
    
    public Vector3 spawnPoint;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    private GameObject GetClosestPlayer(Collider[] colliders)
    {
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < colliders.Length; i++)
        {
            var playerCollider = colliders[i];
            
            float distance = Vector3.Distance(transform.position, playerCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = playerCollider.gameObject;
            }
        }

        return closestPlayer;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        
        var numberPlayersInSight = Physics.OverlapSphere(transform.position, sightRange, WhatIsPlayer);
        var numberPlayersInAttack = Physics.OverlapSphere(transform.position, attackrange, WhatIsPlayer);

        if (numberPlayersInSight.Length != 0)
        {   
            player = GetClosestPlayer(numberPlayersInSight).transform;
            if (Velocity_Z < 5f)
                Velocity_Z +=  acceleration * Time.deltaTime;
            Math.Clamp(Velocity_Z, 2f, 5f);
            playerInSightRange = true;
        }
        else
        {
            if (Velocity_Z > 2f)
                Velocity_Z -=  acceleration * Time.deltaTime * 2;
            Math.Clamp(Velocity_Z, 2f, 5f);
            playerInSightRange = false;
        }
        
        if (numberPlayersInAttack.Length != 0)
            playerInAttackRange = true;
        else
            playerInAttackRange = false;
        
        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) Attack();
        else
            CancelAttack();
        
        agent.speed = Velocity_Z;
    }
    
    float GetFlatDistance(Vector3 a, Vector3 b)
    {
        Vector2 aXZ = new Vector2(a.x, a.z);
        Vector2 bXZ = new Vector2(b.x, b.z);
        return Vector2.Distance(aXZ, bXZ);
    }
    
    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
        else
            agent.SetDestination(walkPoint);

        agent.speed = 2f;
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(spawnPoint, -transform.up, 2f, whatIsGround))
        {
            if (GetFlatDistance(walkPoint, spawnPoint) <= walkPointRange)
                walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player.position);
    }

    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        isAtacking = true;
        if (distanceToPlayer <= distanceFromPlayer)
        {
            blockedByPlayer = true;
            agent.SetDestination(transform.position);
        }
        else
        {
            blockedByPlayer = false;
            agent.SetDestination(player.position);
        }
        
        transform.LookAt(player.position);
    }
    
    
    private void CancelAttack()
    {
        isAtacking = false;
    }
}

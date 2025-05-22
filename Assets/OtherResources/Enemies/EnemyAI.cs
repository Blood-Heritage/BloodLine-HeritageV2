using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
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

    private Collider[] playersInSightRange = new Collider[5];
    private Collider[] playersInAttackRange = new Collider[5];
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private Transform GetClosestPlayer(Collider[] colliders, int hits)
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < hits; i++)
        {
            var playerCollider = colliders[i];
            
            float distance = Vector3.Distance(transform.position, playerCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = playerCollider.transform;
            }
        }

        return closestPlayer;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        
        var numberPlayersInSight = Physics.OverlapSphereNonAlloc(transform.position, sightRange, playersInSightRange, WhatIsPlayer);
        var numberPlayersInAttack = Physics.OverlapSphereNonAlloc(transform.position, attackrange, playersInAttackRange, WhatIsPlayer);

        if (numberPlayersInSight != 0)
        {
            player = GetClosestPlayer(playersInSightRange, numberPlayersInSight);
            playerInSightRange = true;
        }
        
        if (numberPlayersInAttack != 0)
            playerInAttackRange = true;
        
        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

        if (playerInSightRange && playerInAttackRange)
            isAtacking = true;
        else
            isAtacking = false;
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
        else
            agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) 
            walkPointSet = true;
    }


    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player.position);
    }
}

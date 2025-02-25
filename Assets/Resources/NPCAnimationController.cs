using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class NPCAnimationController : MonoBehaviourPun
{
    
    Animator animator;


    public NavMeshAgent agent;

    int isWalkingHash;
    // int isRunningHash;
    private bool moving;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        agent = GetComponent<NavMeshAgent>();
        
        // more performant
        isWalkingHash = Animator.StringToHash("isWalking");
        // isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        if(agent.isStopped){
            moving = false;

        } else {
            moving = true;
        }
        Animation();
    }
    


    void Animation()
    {
        
        bool isWalking = animator.GetBool(isWalkingHash);
        // bool isRunning = animator.GetBool(isRunningHash);
        // if it moves
        if (!isWalking && moving)
        {
            animator.SetBool(isWalkingHash, true);
        }

        // if is not moving
        if (isWalking && !moving)
        {
            animator.SetBool(isWalkingHash, false);
        }
        //
        // if player is walking and presses Shift Left
        //if (!isRunning && (moving && runningKeyPressed))
        //{
        //    animator.SetBool(isRunningHash, true);
        //}
        
        // if player stops walking or running
        //if (isRunning &&  (!moving || !runningKeyPressed))
        //{
        //    animator.SetBool(isRunningHash, false);
        // }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationController : MonoBehaviourPun
{
    
    Animator animator;
    public KeyCode forRunning = KeyCode.LeftShift;
    public KeyCode forShooting = KeyCode.Mouse0;

    int isWalkingHash;
    int isRunningHash;
    int isShootingHash;
    private bool moving;
    // private Rigidbody rb;
    
    Vector3 previousPosition;

    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        previousPosition = transform.position;
        
        // more performant
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isShootingHash = Animator.StringToHash("isShooting");
    }

    void Update()
    {
        if (!photonView.IsMine) return; // Ignore les mouvements des autres joueurs
        
        // Compare the current position with the previous position
        if (Vector3.Distance(previousPosition, transform.position) > 0.01f) moving = true;
        else   moving = false;
        
        Animation();
        
        previousPosition = transform.position;
    }


    void Animation()
    {
        
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        
        bool runningKeyPressed = Input.GetKey(forRunning);
        bool shootingKeyPressed = Input.GetKey(forShooting);
        
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

        // if player is walking and presses Shift Left
        if (!isRunning && (moving && runningKeyPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        
        // if player stops walking or running
        if (isRunning &&  (!moving || !runningKeyPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    
    Animator animator;
    public KeyCode forRunning = KeyCode.LeftShift;

    int isWalkingHash;
    int isRunningHash;
    private bool moving;
    
    Vector3 previousPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
        
        // more performant
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        Debug.Log($"previous position: {previousPosition}, transform.position: {transform.position}, difference: {Vector3.Distance(previousPosition, transform.position)}");
        // Compare the current position with the previous position
        if (Vector3.Distance(previousPosition, transform.position) > 0.01f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        
        Debug.Log($"is moving: {moving}");
        
        Animation();
        
        previousPosition = transform.position;
    }


    void Animation()
    {
        
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        
        bool runningKeyPressed = Input.GetKey(forRunning);
        
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

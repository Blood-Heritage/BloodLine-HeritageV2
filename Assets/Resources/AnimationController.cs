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
    
    
    public void Animation(bool moving)
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

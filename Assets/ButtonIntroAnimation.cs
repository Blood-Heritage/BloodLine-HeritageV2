using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIntroAnimation : MonoBehaviour
{
    private Animator animator;
    private bool hasPlayed = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (!hasPlayed)
        {
            animator.SetTrigger("PlayIntro");
            hasPlayed = true;
        }
        else{
            animator.SetTrigger("Normal");
        }
    }
    public void ResetToNormal()
    {
        animator.Play("Normal"); // Revenir manuellement à Normal si nécessaire
    }
}

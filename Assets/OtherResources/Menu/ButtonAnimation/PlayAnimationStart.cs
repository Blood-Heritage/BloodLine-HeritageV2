using UnityEngine;
using UnityEngine.UI;

public class PlayAnimationStart : MonoBehaviour
{
    public Button myButton;  // Référence au bouton
    public Animator animator; // Référence à l'Animator
    private bool hasPlayed = false; // Booléen pour suivre l'état

    void Start()
    {
        myButton.onClick.AddListener(PlayAnimationOnce);
    }

    void PlayAnimationOnce()
    {
        if (!hasPlayed)
        {
            animator.SetTrigger("Play"); // Déclencher l'animation
            hasPlayed = true; // Empêcher la relecture
        }
    }
}

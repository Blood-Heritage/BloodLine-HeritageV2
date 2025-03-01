using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private Animator animator;
    public Animator animator2;
    private bool isLocked = false;

    [SerializeField] private Button[] buttonsToActivate; // Boutons à activer

    void Start()
    {
        animator = GetComponent<Animator>();

        // Désactive les boutons au début
        foreach (Button btn in buttonsToActivate)
        {
            if (btn != null) btn.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (animator == null) return;
        // Vérifie si l'animation actuelle est "Selected" et qu'elle est terminée
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!isLocked && stateInfo.IsName("Selected") && stateInfo.normalizedTime >= 1.0f)
        {
            isLocked = true; // Marquer comme verrouillé
            MuteAnyStateTransitions(true); // Désactiver les transitions depuis Any State
            ActivateButtons(); // Activer les 3 boutons
        }
    }

    void MuteAnyStateTransitions(bool mute)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false); // Désactive tous les booléens pour bloquer Any State
            }
        }
    }

    void ActivateButtons()
    {
        foreach (Button btn in buttonsToActivate)
        {
            if (btn != null) btn.gameObject.SetActive(true); // Active les boutons spécifiés
        }
    }
    public void retour_menu()
    {
        animator2.SetBool("has_started", true);
        animator2.SetBool("Selected 0", false);
    }

    public void ActivateGameObject(GameObject obj)
    {
        if (obj != null)
            obj.SetActive(true);
    }

    // Désactive un GameObject
    public void DeactivateGameObject(GameObject obj)
    {
        if (obj != null)
            obj.SetActive(false);
    }

    // Quitter le jeu (fonctionne uniquement en build)
    public void QuitGame()
    {
        Debug.Log("Fermeture du jeu...");
        Application.Quit();
    }
}

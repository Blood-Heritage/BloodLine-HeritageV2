using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathCountdown : MonoBehaviour
{
    public Text countdownText;         // Texte UI affichant le compte à rebours
    public GameObject deathScreenUI;   // UI de l'écran de mort
    public string sceneToLoad = "MenuStart"; // Nom de la scène à charger
    public float countdownTime = 5f;  // Temps du compte à rebours

    // desactivation des autres elements de l'UI
    public GameObject HealthBar => BARManager.Instance.healthBar.gameObject;
    public GameObject StaminaBar => BARManager.Instance.staminaBar.gameObject;
    public GameObject PAUSEPANEL => BARManager.Instance.pausePanel.gameObject;


    void OnEnable()
    {
        StaminaBar.SetActive(false);
        HealthBar.SetActive(false);
        PAUSEPANEL.SetActive(false);
        StartCoroutine(CountdownAndRespawn());
    }

    IEnumerator CountdownAndRespawn()
    {
        deathScreenUI.SetActive(true);
        float timer = countdownTime;

        while (timer > 0)
        {
            PAUSEPANEL.SetActive(false);
            countdownText.text = "Respawn in " + Mathf.Ceil(timer) + "...";
            yield return new WaitForSeconds(1f);
            timer--;
        }

        countdownText.text = "Respawning...";

        // Charger la nouvelle scène
        SceneManager.LoadScene(sceneToLoad);
    }
}

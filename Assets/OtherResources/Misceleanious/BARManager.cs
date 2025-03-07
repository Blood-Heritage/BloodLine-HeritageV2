using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

public class BARManager : MonoBehaviourPun
{


    // NE PAS TOUCHER PLEASE
    
    public static BARManager Instance;

    [Header("Health Settings")]
    public float health = 100;
    public float maxHealth = 100;

    [Header("UI")]
    public Image healthBar; // Assign in Inspector
    public GameObject deathUI; // Assign in Inspector

    [SerializeField] private GameObject pausePanel;
    private bool statePause = false;
    
    private bool isDead = false;
    private PhotonView photonView;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AssignPlayer(PhotonView playerView)
    {
        photonView = playerView;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            statePause = !statePause;
            pausePanel.SetActive(statePause);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine|| isDead || photonView == null) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Prevents negative values

        UpdateHealthBar();

        if (health <= 0)
            Die();
    }

    public void Heal(float healAmount)
    {
        if (isDead) return;

        health += healAmount;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
            healthBar.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        health = 0;

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(photonView.gameObject); // Destroy player object

            if (deathUI != null)
                deathUI.SetActive(true);

            StartCoroutine(WaitBeforeSceneChange(5f));
        }
    }

    private IEnumerator WaitBeforeSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay time
        PhotonNetwork.LoadLevel("MenuStart"); // Sync scene transition
    }

    public Image staminaBar; // UI element for the stamina bar

    // Updates the stamina bar UI based on the current stamina
    public void UpdateStaminaBar(float currentStamina, float maxStamina)
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }


}

using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

public class BARManager : MonoBehaviourPun
{
    // NE PAS TOUCHER PLEASE, trop tard
    
    public static BARManager Instance;

    // [Header("Health Settings")]
    // public float health = 100;
    // public float maxHealth = 100;
    public float health => healthComponent.health;
    public float maxHealth => healthComponent.maxHealth;
    
    [Header("UI")] 
    public Image healthBar; // Assign in Inspector
    public GameObject deathUI; // Assign in Inspector
    public Health healthComponent;

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
        healthComponent = playerView.gameObject.GetComponent<Health>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                statePause = !statePause;
                pausePanel.SetActive(statePause);
            }
            if (health <= 0) {
                Die();
            }

            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
            healthBar.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
        }
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
    
    private void Die()
    {
        if (isDead) return;
        isDead = true;
        healthComponent.health = 0;

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(photonView.gameObject); // Destroy player object

            if (deathUI != null)
                deathUI.SetActive(true);

        }
    }

    private IEnumerator WaitBeforeSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay time
        PhotonNetwork.LoadLevel("MenuStart"); // Sync scene transition
    }
    

    
}
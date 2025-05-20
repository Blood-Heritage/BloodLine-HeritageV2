using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

public class BARManager : MonoBehaviourPun
{
    // NE PAS TOUCHER PLEASE, trop tard (Ethan)
    // NE PAS TOUCHER PLEASE, trop tard (Ethan)
    
    public static BARManager Instance;

    // [Header("Health Settings")]
    // public float health = 100;
    // public float maxHealth = 100;
    
    public Stats statsComponent;
    public MovementReborn movementComponent;
    public float health => statsComponent.health;
    public float maxHealth => statsComponent.maxHealth;
    public float stamina => statsComponent.stamina;
    public float maxStamina => statsComponent.maxStamina;
    
    [Header("UI")] 
    public Image healthBar; // Assign in Inspector
    public Image staminaBar; // UI element for the stamina bar
    public GameObject deathUI; // Assign in Inspector
    public GameObject cursorCrosshair;

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
        statsComponent = playerView.gameObject.GetComponent<Stats>();
        movementComponent = playerView.gameObject.GetComponent<MovementReborn>();
        statsComponent = playerView.gameObject.GetComponent<Stats>();
        movementComponent = playerView.gameObject.GetComponent<MovementReborn>();
        
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {

                statePause = !statePause;
                pausePanel.SetActive(statePause);
                movementComponent.canMoveCamera = !movementComponent.canMoveCamera;
            }

            if (statePause)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            UpdateBars();

            if (movementComponent.isAimingPressed)
                cursorCrosshair.SetActive(true);
            else
                cursorCrosshair.SetActive(false);
        }
    }    

    private void UpdateBars()
    {
        UpdateHealthBar();
        UpdateStaminaBar();
    }
    
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
            healthBar.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
        }
    }

    public void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }

    /*
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

            // StartCoroutine(WaitBeforeSceneChange(5f));
        }
    }
    */

    /*
    private IEnumerator WaitBeforeSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay time
        PhotonNetwork.LoadLevel("MenuStart"); // Sync scene transition
    }
    */

}

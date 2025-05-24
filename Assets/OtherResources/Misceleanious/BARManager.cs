using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

using System.Collections;

public class BARManager : MonoBehaviourPun
{
    // NE PAS TOUCHER PLEASE, trop tard (Ethan)

    public static BARManager Instance;

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
    public GameObject TexteQuests;
    public bool ShowQuest = true;

    [SerializeField] public GameObject pausePanel;
    [SerializeField] public Minimap minimap;
    public bool statePause = false;

    private bool isDead => health <= 0;
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
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                minimap.Toggle();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!statePause)
                {
                    ShowQuest = !ShowQuest;
                    TexteQuests.SetActive(ShowQuest);
                }
            }

            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                pauseClickButton();
            }

            UpdateBars();

            if (movementComponent.isAimingPressed || movementComponent.isShootingPressed)
                cursorCrosshair.SetActive(true);
            else
                cursorCrosshair.SetActive(false);
        }
    }

    public void pauseClickButton()
    {
        statePause = !statePause;
        pausePanel.SetActive(statePause);
        movementComponent.pauseIsNotPressed = !movementComponent.pauseIsNotPressed;

        if (statePause)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Time.timeScale = 0f;
            }
            minimap.gameObject.SetActive(false);
            TexteQuests.SetActive(false);   
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            minimap.gameObject.SetActive(true);
            TexteQuests.SetActive(ShowQuest);
            
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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


    private void Die()
    {
        if (photonView.IsMine)
        {
            deathUI.SetActive(true);
        }
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("MenuStart");
    }

}

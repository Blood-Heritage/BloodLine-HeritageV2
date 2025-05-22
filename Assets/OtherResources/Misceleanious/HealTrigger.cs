using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealingZone : MonoBehaviour
{
    public GameObject uiPanel;              // UI Panel avec texte et image
    public Text messageText;                // Texte du message
    public Image hospitalImage;             // Image de l’hôpital
    public Slider loadingBar;               // Barre de chargement

    public GameObject UI;                   // UI 

    private bool isChoosing = false;
    private Stats playerStats;

    private void Start()
    {
        uiPanel.SetActive(false);
        hospitalImage.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isChoosing && playerStats != null && playerStats.photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                StartCoroutine(HealPlayer());
                UI.SetActive(false);
                isChoosing = false;
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                CloseUI();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Stats stats = other.GetComponent<Stats>();
        if (stats != null && stats.photonView.IsMine)
        {
            playerStats = stats;
            uiPanel.SetActive(true);
            messageText.text = "Voulez-vous être soigné ? (Y/N)";
            isChoosing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Stats stats = other.GetComponent<Stats>();
        if (stats != null && stats.photonView.IsMine)
        {
            CloseUI();
            playerStats = null;
        }
    }

    IEnumerator HealPlayer()
    {
        messageText.text = "Soin en cours...";
        hospitalImage.gameObject.SetActive(true);
        loadingBar.gameObject.SetActive(true);
        loadingBar.value = 0;

        float duration = 5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            loadingBar.value = elapsed / duration;
            yield return null;
        }

        playerStats.photonView.RPC("FULL_REGENERATE", RpcTarget.AllBuffered);

        // Message de confirmation
        messageText.text = "Vous avez été soigné !";
        yield return new WaitForSeconds(2f);
        CloseUI();
    }

    private void CloseUI()
    {
        Debug.Log("UI fermée");
        uiPanel.SetActive(false);
        hospitalImage.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(false);
        isChoosing = false;
        UI.SetActive(true);
    }
}

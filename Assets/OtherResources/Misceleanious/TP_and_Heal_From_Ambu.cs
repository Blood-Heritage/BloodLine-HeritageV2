using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TP_andHeal_From_Ambu : MonoBehaviour
{
    public Vector3 teleportPosition = new Vector3(-13.5f, 3f, -140f);
    public Text messageText; // Just the Text GameObject

    private bool isChoosing = false;
    private MovementReborn localPlayer;

    private void Start()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isChoosing && localPlayer != null && localPlayer.photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                TeleportPlayer();
                HideText();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MovementReborn player = other.GetComponent<MovementReborn>();

        if (player != null && player.photonView.IsMine)
        {
            localPlayer = player;
            ShowText("Voulez-vous être téléporté à l'hôpital?\n(Y pour confirmer)");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MovementReborn player = other.GetComponent<MovementReborn>();

        if (player != null && player.photonView.IsMine)
        {
            HideText();
            localPlayer = null;
        }
    }

    private void TeleportPlayer()
    {
        if (localPlayer != null)
        {
            CharacterController cc = localPlayer.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
                localPlayer.transform.position = teleportPosition;
                cc.enabled = true;

                Debug.Log("Joueur local téléporté à " + teleportPosition);
            }
        }
    }

    private void ShowText(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            isChoosing = true;
        }
    }

    private void HideText()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
        isChoosing = false;
    }
}

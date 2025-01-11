using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomNameInputField;
    public GameObject roomListContainer;
    public GameObject roomListItemPrefab;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connexion au serveur Photon
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(); // Rejoindre le lobby
    }

    public void CreateRoom()
    {
        string roomName = roomNameInputField.text; // R�cup�re le texte entr�
        if (!string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Nom de la salle : " + roomName);
            PhotonNetwork.CreateRoom(roomName); // Cr�e une salle avec ce nom
        }
        else
        {
            Debug.LogWarning("Le nom de la salle est vide !");
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene"); // Charge la sc�ne de jeu
    }
}

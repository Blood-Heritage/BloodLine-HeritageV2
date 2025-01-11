using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomNameInputField;  // Le champ pour entrer le nom de la salle
    public GameObject roomListContainer;   // Le container dans lequel les items de la liste vont être ajoutés

    private GameObject roomListItemPrefab; // Le prefab du bouton d'une salle

    void Start()
    {
        // Charger le prefab depuis le dossier Resources
        roomListItemPrefab = Resources.Load<GameObject>("Prefabs/RoomListItem"); // Assurez-vous que le chemin est correct

        if (roomListItemPrefab == null)
        {
            Debug.LogError("Le prefab de l'item de la liste n'a pas été trouvé dans Resources !");
        }

        // Se connecter au serveur Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    // Cette méthode est appelée lorsque nous avons réussi à nous connecter au serveur Photon
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(); // Rejoindre le lobby
    }

    // Cette méthode est appelée pour créer une salle
    public void CreateRoom()
    {
        string roomName = roomNameInputField.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.CreateRoom(roomName); // Crée une salle avec ce nom
        }
        else
        {
            Debug.LogWarning("Le nom de la salle est vide !");
        }
    }

    // Callback de Photon pour récupérer la liste des salles
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Vider le container de salles avant de l'actualiser
        foreach (Transform child in roomListContainer.transform)
        {
            Destroy(child.gameObject); // Supprimer les anciens items
        }

        // Afficher les nouvelles salles disponibles
        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListContainer.transform); // Crée un item de la liste
            Text roomNameText = roomItem.GetComponentInChildren<Text>(); // Récupère le composant Text de l'item
            roomNameText.text = roomInfo.Name; // Définit le nom de la salle

            // Ajouter un bouton pour rejoindre la salle
            Button joinButton = roomItem.GetComponentInChildren<Button>(); // Récupère le bouton
            joinButton.onClick.AddListener(() => JoinRoom(roomInfo.Name)); // Rejoindre la salle quand on clique dessus
        }
    }

    // Méthode pour rejoindre une salle spécifique
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName); // Rejoint la salle
    }

    // Callback appelé lorsque le joueur rejoint une salle
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene"); // Charge la scène de jeu
    }
}

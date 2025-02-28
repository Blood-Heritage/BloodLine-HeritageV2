using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public InputField roomNameInputField;  // Champ pour entrer le nom de la salle
    public GameObject roomListContainer;   // Conteneur des salons dans le ScrollView
    public Button soloButton, createButton; // Boutons pour créer des salons
    public GameObject roomListItemPrefab; // Prefab pour chaque salon

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        soloButton.onClick.AddListener(CreateSoloRoom);
        createButton.onClick.AddListener(CreateRoom);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        string roomName = roomNameInputField.text.Trim();
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.CreateRoom(roomName);
        }
        else
        {
            Debug.LogWarning("Le nom de la salle est vide !");
        }
    }

    public void CreateSoloRoom()
    {
        string randomRoomName = "private_bh_" + Random.Range(1000, 9999);
        PhotonNetwork.CreateRoom(randomRoomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Supprimer les anciens items avant d'ajouter les nouveaux
        foreach (Transform child in roomListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Ajouter chaque salle en tant que bouton
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) continue;
            if(is_not_private(roomInfo.Name)){
                GameObject roomItem = Instantiate(roomListItemPrefab, roomListContainer.transform);
            Text roomNameText = roomItem.GetComponentInChildren<Text>();
            roomNameText.text = roomInfo.Name;

            Button joinButton = roomItem.GetComponent<Button>();
            joinButton.onClick.AddListener(() => JoinRoom(roomInfo.Name));
            }
            
        }
    }

    private bool is_not_private(string roomname){
        if(roomname.Length>=11){
            if(roomname.Substring(0,11)=="private_bh_"){
                return false;
            }
            else{
                return true;
            }
        }
        else{
            return true;
        }

    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Rejoint la salle : {PhotonNetwork.CurrentRoom.Name}");
        PhotonNetwork.LoadLevel("GameScene");
    }
}

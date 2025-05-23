using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public InputField roomNameInputField;
    public GameObject roomListContainer;
    public Button soloButton, createButton;
    public GameObject roomListItemPrefab;

    


    [Header("Loading Video")]
    public GameObject loadingCanvas;
    public GameObject BackgroundCanva;
    public VideoPlayer videoPlayer;
    public VideoClip loopVideo;     // Vidéo boucle (pendant chargement)
    public VideoClip finalVideo;    // Vidéo finale (avant changement de scène)





    [Header("Transition")]
    public GameObject transitionCanvasRoot; // Le GameObject racine qui contient le Canvas + l'image
    public GameObject transitionImage;
    private CanvasGroup transitionCanvasGroup;


    private AsyncOperation asyncLoad;

    void Start()
{
    PhotonNetwork.ConnectUsingSettings();
    print("cliquer sur jouer");

    soloButton.onClick.AddListener(() => StartCoroutine(LoadGameWithVideo(isSolo: true)));
    createButton.onClick.AddListener(() => StartCoroutine(LoadGameWithVideo(isSolo: false)));

    transitionCanvasGroup = transitionCanvasRoot.GetComponentInChildren<CanvasGroup>();
    DontDestroyOnLoad(transitionCanvasRoot); // 👈 ici, sur le Canvas parent
}


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    private IEnumerator LoadGameWithVideo(bool isSolo)
    {
        string roomName = isSolo
            ? "private_bh_" + Random.Range(1000, 9999)
            : roomNameInputField.text.Trim();

        if (!isSolo && string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("Le nom de la salle est vide !");
            yield break;
        }

        // Créer ou rejoindre la salle
        if (isSolo)
            PhotonNetwork.CreateRoom(roomName);
        else
            PhotonNetwork.CreateRoom(roomName);

        // Lancer chargement async de GameScene (sans l’activer)
        asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.allowSceneActivation = false;

        // Activer le Canvas de chargement et démarrer la vidéo en boucle
        loadingCanvas.SetActive(true);
        BackgroundCanva.SetActive(false);
        videoPlayer.clip = loopVideo;
        videoPlayer.isLooping = true;
        videoPlayer.Play();

        // Attendre que GameScene soit chargé à 90% ou plus
        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);

        // Arrêter la vidéo en boucle et lancer la vidéo finale
        videoPlayer.loopPointReached -= OnFinalVideoFinished; // Reset au cas où
        videoPlayer.Stop();
        videoPlayer.clip = finalVideo;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnFinalVideoFinished;
        videoPlayer.Play();
    }

    private void OnFinalVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(TransitionToGameScene());
    }

    private IEnumerator TransitionToGameScene()
    {
        
        transitionImage.SetActive(true);
        transitionCanvasGroup.alpha = 0f;

        // Fondu d'entrée
        float duration = 1f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transitionCanvasGroup.alpha = t / duration;
            yield return null;
        }
        transitionCanvasGroup.alpha = 1f;

        // Activer la scène
        asyncLoad.allowSceneActivation = true;
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in roomListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) continue;
            if (is_not_private(roomInfo.Name))
            {
                GameObject roomItem = Instantiate(roomListItemPrefab, roomListContainer.transform);
                Text roomNameText = roomItem.GetComponentInChildren<Text>();
                roomNameText.text = roomInfo.Name;

                Button joinButton = roomItem.GetComponent<Button>();
                joinButton.onClick.AddListener(() => StartCoroutine(JoinRoomWithVideo(roomInfo.Name)));
            }
        }
    }

    private IEnumerator JoinRoomWithVideo(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);

        // Lancer chargement async de GameScene (sans l’activer)
        asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.allowSceneActivation = false;

        // Activer le Canvas et vidéo boucle
        loadingCanvas.SetActive(true);
        BackgroundCanva.SetActive(false);
        videoPlayer.clip = loopVideo;
        videoPlayer.isLooping = true;
        videoPlayer.Play();

        // Attendre que GameScene soit chargé à 90% ou plus
        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);

        // Lancer vidéo finale
        videoPlayer.loopPointReached -= OnFinalVideoFinished; // reset
        videoPlayer.Stop();
        videoPlayer.clip = finalVideo;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnFinalVideoFinished;
        videoPlayer.Play();
    }

    private bool is_not_private(string roomname)
    {
        return roomname.Length < 11 || roomname.Substring(0, 11) != "private_bh_";
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Rejoint la salle : {PhotonNetwork.CurrentRoom.Name}");
        // Rien ici, géré par coroutine
    }
}

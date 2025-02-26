using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // Assurez-vous d'importer ce namespace

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Référence au composant VideoPlayer
    public string nextSceneName; // Nom de la scène suivante

    void Start()
    {
        // Vérifie si le VideoPlayer est correctement assigné
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Abonne la méthode OnVideoEnd à l'événement loopPointReached
        videoPlayer.loopPointReached += OnVideoEnd;

        // Démarre la vidéo
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Charge la scène suivante une fois la vidéo terminée
        SceneManager.LoadScene(nextSceneName);
    }
}

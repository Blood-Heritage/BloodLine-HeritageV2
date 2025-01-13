using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // Assurez-vous d'importer ce namespace

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // R�f�rence au composant VideoPlayer
    public string nextSceneName; // Nom de la sc�ne suivante

    void Start()
    {
        // V�rifie si le VideoPlayer est correctement assign�
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Abonne la m�thode OnVideoEnd � l'�v�nement loopPointReached
        videoPlayer.loopPointReached += OnVideoEnd;

        // D�marre la vid�o
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Charge la sc�ne suivante une fois la vid�o termin�e
        SceneManager.LoadScene(nextSceneName);
    }
}

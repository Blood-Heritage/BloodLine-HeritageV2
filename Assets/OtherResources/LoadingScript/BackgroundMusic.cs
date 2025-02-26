using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded; // Écoute les changements de scène
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene") // Si on arrive dans Scene2, arrêter la musique
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        else // Si on est dans une autre scène, s'assurer que la musique joue
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Charger la sc�ne du jeu
    }

    public void QuitGame()
    {
        Application.Quit(); // Quitter le jeu (ne fonctionne pas dans l'�diteur)
    }
}

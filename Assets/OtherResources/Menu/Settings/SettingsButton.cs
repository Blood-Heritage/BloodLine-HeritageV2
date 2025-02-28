using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public string url = "https://blood-heritage.github.io/"; // Mets ici l'URL que tu veux ouvrir

    public void OpenWebPage()
    {
        Application.OpenURL(url);
    }
}

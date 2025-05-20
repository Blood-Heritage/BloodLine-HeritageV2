using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OpenLinkOnClick : MonoBehaviour, IPointerClickHandler
{
    // Méthode de gestion du clic
    public void OnPointerClick(PointerEventData eventData)
    {
        // Récupère le texte cliqué
        string clickedText = eventData.pointerPress.GetComponent<TextMeshProUGUI>().text;
        
        // Ouvre l'URL dans un navigateur
        Application.OpenURL("https://blood-heritage.github.io/"); // Remplace par ton lien réel
        
    }
}

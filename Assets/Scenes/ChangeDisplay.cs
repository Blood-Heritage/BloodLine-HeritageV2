using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject[] canvasList; // Liste des Canvas à assigner depuis l'inspecteur
    private int currentCanvasIndex = 0;

    void Start()
    {

        ShowCanvas(currentCanvasIndex); // Affiche le premier Canvas au démarrage
       
    }

    public void ShowCanvas(int index)
    {
        if (index >= 0 && index < canvasList.Length)
        {
            // Désactive tous les Canvas
            foreach (GameObject canvas in canvasList)
            {
                canvas.SetActive(false);
            }

            // Active le Canvas sélectionné
            canvasList[index].SetActive(true);

        }
        else
        {
        }
    }
}

using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject[] canvasList; // Liste des Canvas � assigner depuis l'inspecteur
    private int currentCanvasIndex = 0;

    void Start()
    {

        ShowCanvas(currentCanvasIndex); // Affiche le premier Canvas au d�marrage
       
    }

    public void ShowCanvas(int index)
    {
        if (index >= 0 && index < canvasList.Length)
        {
            // D�sactive tous les Canvas
            foreach (GameObject canvas in canvasList)
            {
                canvas.SetActive(false);
            }

            // Active le Canvas s�lectionn�
            canvasList[index].SetActive(true);

        }
        else
        {
        }
    }
}

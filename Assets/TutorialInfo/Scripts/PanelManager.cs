using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelManager : MonoBehaviour
{
    public GameObject window1; // First window
    public GameObject window2; // Second window
    public GameObject window3; // Third window
    public GameObject window4; // Fourth window

    // Method to open the first window
    public void OpenWindow1()
    {
        CloseAllWindows(); // Close all other windows
        if (window1 != null)
        {
            window1.SetActive(true); // Open Window 1
        }
    }

    // Method to open the second window
    public void OpenWindow2()
    {
        CloseAllWindows(); // Close all other windows
        if (window2 != null)
        {
            window2.SetActive(true); // Open Window 2
        }
    }

    // Method to open the third window
    public void OpenWindow3()
    {
        CloseAllWindows(); // Close all other windows
        if (window3 != null)
        {
            window3.SetActive(true); // Open Window 3
        }
    }

    // Method to open the fourth window
    public void OpenWindow4()
    {
        CloseAllWindows(); // Close all other windows
        if (window4 != null)
        {
            window4.SetActive(true); // Open Window 4
        }
    }

    // Method to close all windows
    public void CloseAllWindows()
    {
        if (window1 != null) window1.SetActive(false);
        if (window2 != null) window2.SetActive(false);
        if (window3 != null) window3.SetActive(false);
        if (window4 != null) window4.SetActive(false);
    }
}



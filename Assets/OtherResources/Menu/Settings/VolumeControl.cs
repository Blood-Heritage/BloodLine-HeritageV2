using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider; // Référence au Slider

    void Start()
    {
        // Initialiser la valeur du Slider avec le volume actuel
        volumeSlider.value = AudioListener.volume;

        // Ajouter un écouteur pour détecter les changements de valeur du Slider
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    void ChangeVolume(float volume)
    {
        // Mettre à jour le volume global de l'application
        AudioListener.volume = volume;
    }
}
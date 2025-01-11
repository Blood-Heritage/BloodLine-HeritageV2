using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Le personnage à suivre
    public Vector3 offset;   // Décalage de la caméra par rapport au personnage

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}

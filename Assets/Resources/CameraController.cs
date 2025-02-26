using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Le personnage � suivre
    public Vector3 offset;   // Decalage de la cam�ra par rapport au personnage

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}

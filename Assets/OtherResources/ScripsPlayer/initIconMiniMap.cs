using UnityEngine;

public class initIconMiniMap : MonoBehaviour
{
    [Header("Minimap")]
    public GameObject CanvasPlayer;
    public GameObject minimapIcon;
        
    private void Start()
    {
        Instantiate(minimapIcon, CanvasPlayer.transform);
    }
}

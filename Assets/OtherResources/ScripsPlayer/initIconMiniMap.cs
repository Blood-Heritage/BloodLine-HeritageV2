using UnityEngine;

public class initIconMiniMap : MonoBehaviour
{
    [Header("Minimap")]
    public GameObject CanvasPlayer;
    public GameObject minimapIcon;
    
    public GameObject Image;
        
    private void Start()
    {
        Image = Instantiate(minimapIcon, CanvasPlayer.transform);
        // transform.SetParent(PlayerContainer.Instance.instanceTransform, true);
    }
}
using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    private void Start()
    {
        if (photonView.IsMine && BARManager.Instance != null)
        {
            BARManager.Instance.AssignPlayer(photonView);
        }
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;

        if (BARManager.Instance != null)
            BARManager.Instance.TakeDamage(damage);
    }
}

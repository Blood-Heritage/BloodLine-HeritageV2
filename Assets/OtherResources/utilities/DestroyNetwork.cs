using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class DestroyNetwork : MonoBehaviourPun
{
    public bool IsOnline()
    {
        return PhotonNetwork.IsConnected;
    }
    
    public virtual void DestroyOnNetwork()
    {
        if (IsOnline())
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
                photonView.RPC("NetworkDestroy", RpcTarget.Others);
        }
        else
        {
            // for local dev
            Destroy(gameObject);
        }
    }

    [PunRPC]
    public void NetworkDestroy()
    {
        Destroy(this.gameObject);
    }
}

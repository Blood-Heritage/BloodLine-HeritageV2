using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyAnimation : MonoBehaviourPun
{
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
    }
    
    
}

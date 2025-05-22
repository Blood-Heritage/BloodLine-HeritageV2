using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyContainer : MonoBehaviour
{
    public EnemyContainer Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("There's already a EnemyContainer in the scene STUPID");
            Destroy(gameObject);
        }
    }

    public int CountEnemies()
    {
        return 0;
    }
}

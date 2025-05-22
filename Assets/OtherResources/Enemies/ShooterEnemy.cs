using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ShooterEnemy : MonoBehaviourPun
{
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    // [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] public EnemyAI enemy;
    public Transform lookingAt;

    
    public int damage;
    public float fireRate;
    private float nextFire;
    
    private void Awake()
    {
        enemy = GetComponent<EnemyAI>();
    }

    void Update()
    {
        // ignore if not the owner
        if (!photonView.IsMine) return;
        
        
        if (nextFire > 0)
            nextFire -= Time.deltaTime;

        if (enemy.isAtacking && nextFire <= 0)
        {
            Fire();
        }
    }


    void Fire()
    {
        Vector3 aimDir = (lookingAt.position - spawnBulletPosition.position).normalized;
        PhotonNetwork.Instantiate("bullet", spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        
        nextFire = 1 / fireRate;
    }
}

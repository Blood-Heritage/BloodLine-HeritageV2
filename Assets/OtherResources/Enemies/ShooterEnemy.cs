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
    [SerializeField] private GameObject pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] public EnemyAI enemy;
    public Transform lookingAt;

    public float fireRate;
    private float nextFire = 0f;
    
    private void Awake()
    {
        enemy = GetComponent<EnemyAI>();
    }

    void Update()
    {
        // ignore if not the owner
        if (!photonView.IsMine) return;
        if (enemy.isDead) return;
        
        nextFire -= Time.deltaTime;
        if (nextFire <= 0f && enemy.isAtacking)
        {
            Fire();
            nextFire = 1f / fireRate;
        }    
    }


    void Fire()
    {
        Vector3 aimDir = (lookingAt.position - spawnBulletPosition.position + new Vector3(0, 0.1f, 0)).normalized;
        // PhotonNetwork.Instantiate("bullet", spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        
        // testing
        PhotonNetwork.Instantiate("bullet_5_damage", spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        
        // nextFire = 1 / fireRate;
    }
}

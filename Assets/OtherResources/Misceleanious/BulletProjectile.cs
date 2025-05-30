using System;
using System.Collections;
using System.Collections.Generic;
using OtherResources.Interfaces;
using Photon.Pun;
using UnityEngine;
using static OtherResources.Constants;

public class BulletProjectile : DestroyNetwork
{
    public float bulletSpeed;
    public int damage;
    private float timer = 0;
    private float maxLife = 5f;
    
    private Rigidbody bulletRigidBody;
    [SerializeField] private GameObject impactPrefab;
    private PhotonView photonView;
    
    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        bulletRigidBody.velocity = transform.forward * bulletSpeed;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxLife)
        {
            bulletRigidBody.velocity = transform.forward * 0;
            DestroyOnNetwork();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        if (other.gameObject.layer == IGNORE) return;
        
        // not enable the bullet to move anymore
        bulletRigidBody.velocity = transform.forward * 0;
        DestroyOnNetwork();
        
        Instantiate(impactPrefab, transform.position, Quaternion.identity);

        // if the other object has the tag player
        if (other.gameObject.layer == ENEMY_LAYER || other.gameObject.layer == PLAYER_LAYER)
        {
          // asumme that it has a Health Component

          PhotonView otro = other.gameObject.GetComponent<PhotonView>();
          if (otro == null)
          {
            Debug.LogError("No PhotonView Associated with player");
            return;
          }

          otro.RPC("TakeDamage", RpcTarget.AllBuffered, damage);
          Debug.Log("A player was hit");
        }

    }
}

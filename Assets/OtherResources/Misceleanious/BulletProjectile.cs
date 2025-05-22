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
            DestroyOnNetwork();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == IGNORE) return;
        
        // not enable the bullet to move anymore
        bulletSpeed = 0f;
        
        DestroyOnNetwork();
        
        if (IsOnline())
        {
            if (!photonView.IsMine) return;
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
        else
        {
            if (other.gameObject.layer == ENEMY_LAYER || other.gameObject.layer == PLAYER_LAYER)
            {
                IHealth health = other.gameObject.GetComponent<IHealth>();
                if (health == null)
                    Debug.LogError("No stats component was found");
                else
                {
                    health.TakeDamage(damage);
                    Debug.Log($"Health left: {health.health}");
                }
            }
            else
                Debug.Log("Not Vulnerable :(");
            
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
        }
    }
}

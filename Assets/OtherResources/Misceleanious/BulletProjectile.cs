using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
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
        if (photonView.IsMine)
        {
            timer += Time.deltaTime;
            if (timer >= maxLife)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonNetwork.Destroy(gameObject);
        if (photonView.IsMine)
        {
            Instantiate(impactPrefab, transform.position, Quaternion.identity);

            // if the other object has the tag player
            if (other.gameObject.CompareTag("Player"))
            {
                PhotonView otro = other.gameObject.GetComponent<PhotonView>();
                if (otro == null)
                {
                    Debug.LogError("No PhotonView Associated with player");
                    return;
                }
                
                otro.RPC("TakeDamage", RpcTarget.AllBuffered, damage);
                Debug.Log("A player was hit");
                
                // var health = other.GetComponent<Health>();
                // if (health == null) Debug.LogError("Player doesn't have health component");
                // else health.TakeDamage(damage);
                //
            }
        }
        
    }
}

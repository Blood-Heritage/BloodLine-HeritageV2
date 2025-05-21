using System;
using System.Collections;
using System.Collections.Generic;
using OtherResources.Interfaces;
using Photon.Pun;
using UnityEngine;
using static OtherResources.Constants;

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

    public bool IsOnline()
    {
        return PhotonNetwork.IsConnected && PhotonNetwork.InRoom;
    }

    private void Start()
    {
        bulletRigidBody.velocity = transform.forward * bulletSpeed;
    }

    private void Update()
    {
        if (IsOnline())
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
        else
        {
            timer += Time.deltaTime;
            if (timer >= maxLife)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOnline())
        {
            if (!photonView.IsMine) return;
            PhotonNetwork.Destroy(gameObject);
            
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
                
            // if the other object has the tag player
            if (other.gameObject.layer == VULNERABLE_LAYER)
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
            Destroy(gameObject);

            if (other.gameObject.layer == VULNERABLE_LAYER)
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

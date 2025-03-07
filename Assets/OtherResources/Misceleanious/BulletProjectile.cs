using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float bulletSpeed;
    private Rigidbody bulletRigidBody;
    [SerializeField] private GameObject impactPrefab;

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidBody.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(impactPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        
        // TODO:  check if colided with health object if yes -= damage
    }
}

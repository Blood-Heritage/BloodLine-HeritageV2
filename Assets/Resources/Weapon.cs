using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Photon.Pun;
using UnityEngine;

public class weapon : MonoBehaviour
{

    public Camera shootingCamera;
    public int damage;
    public float fireRate;
    private float nextFire;

    // Update is called once per frame
    void Update()
    {
        if (nextFire > 0)
            nextFire -= Time.deltaTime;
        
        if (Input.GetButtonDown("Fire1") && nextFire <= 0)
        {
            nextFire = 1 / fireRate;
            Fire();
        }
        
    }

    void Fire()
    {
        Ray ray = new Ray(shootingCamera.transform.position, shootingCamera.transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }  
        }
    }
}

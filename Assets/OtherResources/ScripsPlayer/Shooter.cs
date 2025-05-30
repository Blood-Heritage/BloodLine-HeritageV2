using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    MovementReborn movementReborn;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    // [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    public Transform mouseWorldPosition;
    
    
    public int damage;
    public float fireRate;
    private float nextFire;
    private bool online;
    private void Awake()
    {
        movementReborn = GetComponent<MovementReborn>();
    }

    private void Start()
    {
        online = movementReborn.IsOnline();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementReborn.photonView.IsMine )
        {

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                // debugTransform.position = raycastHit.point;
                mouseWorldPosition.position = raycastHit.point;
            }
            
            if (nextFire > 0)
                nextFire -= Time.deltaTime;

            if (movementReborn.isShootingPressed && nextFire <= 0 && movementReborn.pauseIsNotPressed)
            {
                Fire();
            }
        }
    }


    void Fire()
    {
        Vector3 aimDir = (mouseWorldPosition.position - spawnBulletPosition.position).normalized;
        if (online)
        {
            // PhotonNetwork.Instantiate(pfBulletProjectile);
            PhotonNetwork.Instantiate("bullet", spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
        else
        {
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
        
        nextFire = 1 / fireRate;
    }
}

using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class weapon : MonoBehaviour
{

    public GameObject impactVFX;
    public Transform orientationShooting;
    public KeyCode shootButton;
    public int damage;
    public float fireRate;
    private float nextFire;
    // public Animator animator;
    // private int isShootingHash;
    
    public InputActionReference shoot;
    private float _shoot;



    // Update is called once per frame
    void Update()
    {
        
        _shoot = shoot.action.ReadValue<float>();
        
        if (nextFire > 0)
            nextFire -= Time.deltaTime;
        
        if (_shoot > 0.1f && nextFire <= 0)
        {
            // animator.SetBool(isShootingHash, true);
            Debug.Log("Fire");
            nextFire = 1 / fireRate;
            Fire();
        }
        
        // if (_shoot < 0.1f) animator.SetBool(isShootingHash, false);
    }

    void Fire()
    {
        Ray ray = new Ray(orientationShooting.position, orientationShooting.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            Debug.Log("Hit");
            Debug.Log(hit.collider.gameObject.name);
            
            // var effect = Instantiate(impactVFX, hit.point, Quaternion.LookRotation(hit.normal));
            
            if (hit.transform.gameObject.GetComponent<Health>())
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
             
        }
        else
        {
            Debug.Log("No Hit :(");
        }
    }
}

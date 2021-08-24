using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTesting : MonoBehaviour
{
    public GameObject testingProjectile;
    public Transform ProjectileSpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Shoot();
        }
    }

    public void Shoot()
    {    

        var projectile = Instantiate(testingProjectile, ProjectileSpawnLocation.position, Quaternion.identity, GameManager.Instance.projectileHolder.transform);
    }
}

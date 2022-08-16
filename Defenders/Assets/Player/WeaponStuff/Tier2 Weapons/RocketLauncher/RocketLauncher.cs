using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : BasicWeaponScript
{

    public GameObject rocket;
    public Transform rocketSpawn;
    

    public override void EquipGun()
    {
        throw new System.NotImplementedException();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (lastShootTime + shootDelay < Time.time)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
                {
                    Transform test = bulletSpawnPoint.transform;
                    test.transform.LookAt(hit.point);
                    Instantiate(rocket, bulletSpawnPoint.transform.position, test.rotation);
                    //print("test");
                } else
                {
                    Instantiate(rocket, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                }
                
                
                lastShootTime = Time.time;
                currentNumOfBullets -= 1;
                //spawn stuff

                AddRecoil();
            }

        }
        ControlRecoil();
    }

    
}

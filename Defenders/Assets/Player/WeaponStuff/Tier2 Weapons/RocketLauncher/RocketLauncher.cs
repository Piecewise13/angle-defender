using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : BasicWeaponScript
{

    public GameObject rocketPrefab;
    public Transform rocketSpawn;
    

    public override void EquipGun()
    {
        setUp = true;
        canShoot = false;
        transform.localPosition = Vector3.zero;

        //print(transform.localPosition);
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
                    RocketScript script = Instantiate(rocketPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation).GetComponent<RocketScript>();
                    script.destination = hit.point;

                    //print("test");
                } else
                {
                    RocketScript script = Instantiate(rocketPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation).GetComponent<RocketScript>();
                    script.destination = bulletSpawnPoint.transform.position + playerCamera.transform.TransformDirection(Vector3.forward) * 1000f;

                }
                
                
                lastShootTime = Time.time;
                currentNumOfBullets -= 1;
                //spawn stuff

                AddRecoil();
                player.SetSoulFire(-bulletCost);
            }

        }
        ControlRecoil();
    }

    
}

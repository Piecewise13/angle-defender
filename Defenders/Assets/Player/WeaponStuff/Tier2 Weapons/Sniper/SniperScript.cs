using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScript : BasicWeaponScript
{

    bool isAiming;
    bool hasBullet = true;

    public ParticleSystem bulletSystem;

    public override void EquipGun()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //Shoot weapon
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            if (hasBullet && !isReloading)
            {
                Shoot();
                print("shoot");

                Reload();
            }
            else
            {
                //play out of bullets sound
            }
        }

        if (Input.GetButtonDown("Fire2") && canShoot)
        {
            StartAim();
            isAiming = true;
            player.lookScript.bUseAimSens = true;
        }

        if (Input.GetButtonUp("Fire2") && canShoot)
        {
            isAiming = false;
            StopAim();
        }

        //bring recoil back to center
        ControlRecoil();

        if (Input.GetButtonDown("Reload") && currentNumOfBullets != clipSize)
        {
            Reload();

        }


        //set up gun when it's first equiped
        if (setUp)
        {
            if (setupTimer > weaponSetUpTime)
            {

                setUp = false;
                canShoot = true;
                setupTimer = 0;
                print("set up");
            }
            setupTimer += Time.deltaTime;
        }

        if (isReloading)
        {
            FinishReload();
        }
    }


    new public void Shoot()
    {
        RaycastHit hit;
        Vector3 aimingDirection;

        if (isAiming)
        {
            aimingDirection = playerCamera.transform.TransformDirection(Vector3.forward);
        } else
        {
            aimingDirection = playerCamera.transform.TransformDirection(Vector3.forward) + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        }

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
        {
            trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
            //print("hit gameobject: " + hit.collider.gameObject);
            StartCoroutine(SpawnTrail(trailObject, hit.point));
            try
            {

                Damageable hitGameobject = hit.collider.gameObject.GetComponentInParent<Damageable>();
                //print("damageable gameobject: " + hitGameobject);


                hitGameobject.TakeDamage(damage, hit.collider);
                //print(damage * damageMultiplier);

            }
            catch (System.Exception)
            {

            }


        }

        AddRecoil();
        hasBullet = false;
    }

    new void Reload()
    {
        bulletSystem.Stop();
        StopAim();
        canShoot = false;
        isReloading = true;
        startReloadTime = Time.time;
        if (player.GetSoulFire() >= bulletCost)
        {
            playerAnimator.SetBool("isReloading", true);

        }

    }

    new void FinishReload()
    {
        if (reloadDuration + startReloadTime < Time.time)
        {
            playerAnimator.SetBool("isReloading", false);
            isReloading = false;
            canShoot = true;
            hasBullet = true;
            bulletSystem.Play();
        }

    }

}

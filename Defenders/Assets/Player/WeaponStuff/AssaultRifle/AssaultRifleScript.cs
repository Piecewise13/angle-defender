using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleScript : BasicWeaponScript
{

    
    TrailRenderer trailObject;

    // Update is called once per frame
    void Update()
    {
        //Shoot weapon
        if (Input.GetButton("Fire1") && canShoot)
        {
            if (currentNumOfBullets != 0)
            {
                if (shootDelay + lastShootTime < Time.time)
                {
                    //print("shooting");

                    RaycastHit hit;
                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
                    {
                        trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
                        //print("hit gameobject: " + hit.collider.gameObject);
                        StartCoroutine(spawnTrail(trailObject, hit));
                        try
                        {

                            Damageable hitGameobject = hit.collider.gameObject.GetComponentInParent<Damageable>();
                            //print("damageable gameobject: " + hitGameobject);


                            hitGameobject.takeDamage(damage * (1 + damageMultiplier), hit.collider);
                            //print(damage * damageMultiplier);

                        }
                        catch (System.Exception)
                        {

                        }


                    }

                    lastShootTime = Time.time;
                    Recoil();
                    currentNumOfBullets -= 1;
                    UpdateHUD();
                }
            } else
            {
                //play out of bullets sound
            }
        }

        //bring recoil back to center
        ControlRecoil();

        if (Input.GetButtonDown("Reload") && currentNumOfBullets != clipSize)
        {
            Reload();
            //play animation
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
            if (reloadDuration + startReloadTime < Time.time)
            {
                isReloading = false;
                canShoot = true;
            }
        }
    }

    public IEnumerator spawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        //animator.SetBool("isShooting", false);
        trail.transform.position = hit.point;
        //Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(trail.gameObject, trail.time);
        
    }

    private void OnDestroy()
    {
        if (trailObject != null)
        {
            Destroy(trailObject.gameObject);
            
        }

    }

    public override void EquipGun()
    {
        
        setUp = true;
        canShoot = false;
        transform.localPosition = Vector3.zero;
        
        //print(transform.localPosition);

    }
}


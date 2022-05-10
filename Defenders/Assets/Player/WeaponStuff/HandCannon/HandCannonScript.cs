using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCannonScript : BasicWeaponScript
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            if (currentNumOfBullets != 0) {
                if (shootDelay + lastShootTime < Time.time)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
                    {
                        TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);

                        StartCoroutine(spawnTrail(trail, hit));
                        try
                        {
                            Damageable hitGameobject = hit.collider.gameObject.GetComponentInParent<Damageable>();


                            hitGameobject.takeDamage(damage, hit.collider);
                        }

                        catch (System.Exception)
                        {

                        }


                    }

                    lastShootTime = Time.time;
                    currentNumOfBullets -= 1;
                    UpdateHUD();

                    Recoil();

                }
            } else
            {
                //PLAY RELOAD SOUNDS
            }


        }
        ControlRecoil();

        if(Input.GetButtonDown("Reload") && currentNumOfBullets != clipSize)
        {
            Reload();
            //PLAY RELOAD ANIM
        }

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

        while (time < 1)
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

    public override void EquipGun()
    {
        //animator stuff
        setUp = true;
        canShoot = false;
        transform.localPosition = Vector3.zero;
        UpdateHUD();
        //print(transform.localPosition);
    }
}

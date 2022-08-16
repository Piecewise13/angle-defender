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
                    Shoot();

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


        /*
         * AIMING STUFF
         */ 
        if (Input.GetButtonDown("Fire2") && canShoot)
        {
            StartAim();
        }

        if (Input.GetButtonUp("Fire2") && canShoot)
        {
            StopAim();
        }

        if (setUp)
        {
            if (setupTimer > weaponSetUpTime)
            {

                setUp = false;
                canShoot = true;
                setupTimer = 0;
            }
            setupTimer += Time.deltaTime;
        }

        if (isReloading)
        {
            FinishReload();
        }
    }


    public override void EquipGun()
    {
        //animator stuff
        setUp = true;
        canShoot = false;
        transform.localPosition = Vector3.zero;
        //print(transform.localPosition);
    }
}

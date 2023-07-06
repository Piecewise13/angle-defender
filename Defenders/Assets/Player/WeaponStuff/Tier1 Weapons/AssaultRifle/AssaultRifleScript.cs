using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleScript : ParentWeaponScript
{

    public ParticleSystem bulletSystem;

    // Update is called once per frame
    private void Update()
    {
        //Shoot weapon
        if (Input.GetButton("Fire1") && canShoot)
        {
            if (currentNumOfBullets != 0)
            {
                if (shootDelay + lastShootTime < Time.time)
                {
                    UpdateParticleSystem();
                    Shoot();
                }
            } else
            {
                //play out of bullets sound
            }
        }

        if (Input.GetButtonDown("Fire2") && canShoot)
        {
            StartAim();
        }

        if (Input.GetButtonUp("Fire2") && canShoot)
        {
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


    protected override void FinishReload()
    {
        base.FinishReload();
        bulletSystem.transform.localScale = Vector3.one;
    }

    protected void UpdateParticleSystem()
    {
        bulletSystem.transform.localScale = Vector3.Lerp(Vector3.one * .1f, Vector3.one, (float)currentNumOfBullets / (float)clipSize);



    }

    public override void EquipGun()
    {
        
        setUp = true;
        canShoot = false;
        transform.localPosition = Vector3.zero;
        
        //print(transform.localPosition);

    }
}


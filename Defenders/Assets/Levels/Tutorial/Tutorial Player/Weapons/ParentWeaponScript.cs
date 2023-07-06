using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;

namespace Tutorial
{

    public abstract class Tutorial_ParentWeaponScript : ParentWeaponScript
    {

        new Tutorial_PlayerScript player;

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
            player = GetComponentInParent<Tutorial_PlayerScript>();
        }


        protected new void Shoot()
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
            {
                trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
                //print("hit gameobject: " + hit.collider.gameObject);
                StartCoroutine(SpawnTrail(trailObject, hit.point));
                Damageable hitGameobject = hit.collider.gameObject.GetComponentInParent<Damageable>();
                if (hitGameobject != null)
                {
                    float damageGiven;
                    bool crit;
                    hitGameobject.GiveDamage(damage * damageMultiplier, hit.collider, out damageGiven, out crit);

                    SpawnDamageIndicator(hit.point, damageGiven, crit);

                }

            }
            else
            {

                trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trailObject, playerCamera.transform.position + playerCamera.transform.forward * 500f));
            }


            lastShootTime = Time.time;
            AddRecoil();
            currentNumOfBullets -= 1;
            
            player.hudScript.UpdateBulletCount(currentNumOfBullets);
            player.shootingRangeScript.DisplayNextText(TUTORIAL_STEPS.SHOOT);

        }

        public new void Reload()
        {
            player.shootingRangeScript.DisplayNextText(TUTORIAL_STEPS.RELOAD);
            StopAim();
            int numBullets = player.GetSoulFire();
            if (bulletCost > 0)
            {
                if (numBullets <= 0)
                {
                    numBullets = 0;
                    return;
                }
                if (numBullets < bulletCost)
                {
                    return;
                }
            }

            if ((clipSize - currentNumOfBullets) * bulletCost > numBullets)
            {
                currentNumOfBullets += numBullets / bulletCost;
                player.SetSoulFire(-numBullets + (numBullets % bulletCost));
            }
            else
            {
                player.SetSoulFire(-1 * (clipSize - currentNumOfBullets) * bulletCost);
                currentNumOfBullets = clipSize;
            }

            canShoot = false;
            isReloading = true;
            startReloadTime = Time.time;
            playerAnimator.SetBool("isReloading", true);



        }


        public void SetPlayer(Tutorial_PlayerScript script)
        {
            player = script;
        }

        protected new void StartAim()
        {
            playerAnimator.SetBool("isAiming", true);
            player.shootingRangeScript.DisplayNextText(TUTORIAL_STEPS.AIM);
            //player.ChangeCameraZoom(adsZoom);
        }
    }
}
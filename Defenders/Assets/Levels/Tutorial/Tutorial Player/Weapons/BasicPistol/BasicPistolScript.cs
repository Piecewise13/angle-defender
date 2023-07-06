using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class BasicPistolScript : Tutorial_ParentWeaponScript
    {

        public GameObject[] soulFireTubes;

        public override void EquipGun()
        {
            setUp = true;
            canShoot = false;
            transform.localPosition = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            //Shoot weapon
            if (Input.GetButtonDown("Fire1") && canShoot)
            {
                if (currentNumOfBullets != 0)
                {
                    SoulFireTubeHandler();
                    Shoot();

                }
                else
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
            for (int i = 0; i < soulFireTubes.Length; i++)
            {
                soulFireTubes[i].SetActive(true);
            }
        }

        private void SoulFireTubeHandler()
        {
            soulFireTubes[clipSize - currentNumOfBullets].SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tutorial {

    public class Player_ModeManager : ModeManager
    {

        new Tutorial_PlayerScript player;

        new void Start()
        {
            base.Start();
            player = GetComponent<Tutorial_PlayerScript>();
        }

        // Update is called once per frame
        new void Update()
        {
            if (Input.GetButtonDown("SwitchMode"))
            {
                CycleMode();
            }

            if (!freeToPlay)
            {
                return;
            }
            WallDefenceScript.showIndicators = false;
            switch (mode)
            {

                case PlayerMode.Weapons:
                    if (Input.GetKeyDown("1"))
                    {
                        ChangeGun(0);

                    }
                    if (Input.GetKeyDown("2"))
                    {
                        ChangeGun(1);
                    }
                    break;
                case PlayerMode.Defense:
                    if (Input.GetKeyDown("1"))
                    {
                        if (isDefenseUtility)
                        {
                            UnequipBuildingUtility();
                        }
                        ChangeDefense(0);
                    }
                    if (Input.GetKeyDown("2"))
                    {
                        if (isDefenseUtility)
                        {
                            UnequipBuildingUtility();
                        }
                        ChangeDefense(1);
                    }
                    if (Input.GetKeyDown("3"))
                    {
                        if (isDefenseUtility)
                        {
                            UnequipBuildingUtility();
                        }
                        ChangeDefense(2);
                    }
                    if (Input.GetKeyDown("4"))
                    {
                        if (isDefenseUtility)
                        {
                            UnequipBuildingUtility();
                        }
                        ChangeDefense(3);
                    }

                    if (Input.GetButtonDown("Utility"))
                    {
                        if (isDefenseUtility)
                        {
                            UnequipBuildingUtility();
                        }
                        else
                        {
                            EquipBuildingUtility();
                        }

                    }


                    //If removing, then bring out the gun an
                    if (isDefenseUtility)
                    {
                        if (Input.GetButtonDown("Fire1") && freeToPlay)
                        {
                            playerAnimator.SetTrigger("UtilityUse");

                            RaycastHit hit1;
                            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit1, defenseUtilityRange, defenseLayer))
                            {
                                WallDefenceScript wallScript = hit1.collider.GetComponentInParent<WallDefenceScript>();
                                if (wallScript == null)
                                {
                                    break;
                                }
                                dataManager.WallDestroyed(wallScript.gameObject);
                                wallScript.Death();

                            }
                        }
                        break;
                    }


                    bool validPlacement = true;
                    hitTowerWall = null;
                    WallDefenceScript.showIndicators = false;

                    defenseGhost.transform.position = placeLocation;
                    if (activeDefense == 0)
                    {
                        WallDefenceScript.showIndicators = true;
                        RaycastHit hitout;

                        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitout, range, defenseLayer))
                        {
                            if (!isLatched)
                            {
                                if (hitout.collider.CompareTag("Ground"))
                                {
                                    if (!freezeMovement)
                                    {
                                        placeLocation = hitout.point;
                                    }
                                    WallDefenceScript.showIndicators = true;
                                    validPlacement = true;
                                }
                                else if (hitout.collider.CompareTag("Wall"))
                                {
                                    WallDefenceScript.showIndicators = false;
                                    isLatched = true;
                                    placeLocation = (hitout.collider.transform.position + hitout.collider.transform.forward.normalized * 4).xz3();

                                    hitCollider = hitout.collider;
                                    defenseGhost.transform.LookAt(hitCollider.transform.position.xz3(), Vector3.up);
                                    validPlacement = true;
                                }
                            }

                            if (Input.GetButtonDown("RotateDefense"))
                            {
                                if (!isLatched)
                                {
                                    freezeMovement = true;
                                    placeLocation = hitout.point;
                                }
                            }

                            if (Input.GetButtonUp("RotateDefense"))
                            {
                                freezeMovement = false;
                            }

                            if (Input.GetButton("RotateDefense"))
                            {
                                if (isLatched)
                                {

                                    WallDefenceScript.showIndicators = false;

                                    RaycastHit rotHit;
                                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rotHit, 100f, LayerMask.GetMask("Ground")))
                                    {  
                                        Vector3 dir = (rotHit.point.xz3() - hitCollider.transform.position.xz3()).normalized;
                                        Vector3 location = (hitCollider.transform.position.xz3() + dir * 4);
                                        placeLocation = Extns.LocationWithOthersHeight(location, hitCollider.transform.root.position);
                                        player.miniGameScript.DisplayText(TUTORIAL_STEPS.ROTATE_DEFENSES2 + 1);
                                    }
                                    else
                                    {
                                        WallDefenceScript.showIndicators = true;
                                        isLatched = false;
                                    }

                                    defenseGhost.transform.LookAt(Extns.LocationWithOthersHeight(hitCollider.transform.position, hitCollider.transform.root.position), Vector3.up);

                                }
                                else
                                {
                                    RaycastHit rotHit;
                                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rotHit, 100f, LayerMask.GetMask("Ground")))
                                    {
                                        defenseGhost.transform.LookAt(hitout.point.xz3() + defenseGhost.transform.position.y * Vector3.up, Vector3.up);
                                        player.miniGameScript.DisplayText(TUTORIAL_STEPS.ROTATE_DEFENSES1 + 1);
                                    }
                                }
                            }
                            else
                            {
                                isLatched = false;

                            }
                        }
                        else
                        {
                            validPlacement = false;
                            isLatched = false;
                        }
                    }
                    else if (activeDefense == 1)
                    {

                        player.miniGameScript.DisplayText(TUTORIAL_STEPS.ENTER_TURRET + 1);
                        RaycastHit towerHit;
                        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out towerHit, range, LayerMask.GetMask("Defense", "Ground")))
                        {
                            if (towerHit.collider.gameObject.CompareTag("Wall"))
                            {
                                hitTowerWall = towerHit.collider.transform.root.gameObject;
                                validPlacement = !dataManager.TowerAlreadyPlacedOnWall(hitTowerWall);
                                placeLocation = towerHit.point.xz3();
                                defenseGhost.transform.rotation = towerHit.collider.transform.root.rotation;

                            }
                            else if (towerHit.collider.gameObject.CompareTag("Ground"))
                            {
                                validPlacement = false;
                                placeLocation = towerHit.point;
                            }
                            else
                            {
                                validPlacement = false;
                            }
                        }
                        else
                        {
                            validPlacement = false;
                        }
                    }
                    else if (activeDefense == 2)
                    {
                        RaycastHit hitout;
                        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitout, range, defenseLayer))
                        {
                            if (!isLatched)
                            {
                                if (hitout.collider.CompareTag("Ground"))
                                {
                                    print("world location");
                                    placeLocation = hitout.point;
                                    validPlacement = true;
                                }
                                else if (hitout.collider.CompareTag("Wall"))
                                {
                                    isLatched = true;
                                    print("hitting wall");
                                    placeLocation = (hitout.collider.transform.position + hitout.collider.transform.forward.normalized * 4).xz3();
                                    hitCollider = hitout.collider;
                                    validPlacement = true;
                                }
                            }

                            if (Input.GetButton("RotateDefense"))
                            {
                                if (isLatched)
                                {
                                    /*OLD METHOD
                                    latchAngle += defenseRotationSpeed * Time.deltaTime;
                                    */
                                    RaycastHit rotHit;
                                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rotHit, 100f, LayerMask.GetMask("Ground")))
                                    {
                                        Vector3 dir = (rotHit.point.xz3() - hitCollider.transform.position.xz3()).normalized;
                                        Vector3 location = (hitCollider.transform.position.xz3() + dir * 4);
                                        placeLocation = location;
                                    }
                                    else
                                    {
                                        isLatched = false;
                                    }

                                    defenseGhost.transform.LookAt(hitCollider.transform.position.xz3(), Vector3.up);

                                }
                                else
                                {
                                    defenseGhost.transform.Rotate(Vector3.up * (defenseRotationSpeed * Time.deltaTime));
                                }
                            }
                            else
                            {
                                isLatched = false;
                            }
                        }
                        else
                        {
                            validPlacement = false;
                            isLatched = false;
                        }

                    }
                    else if (activeDefense == 3)
                    {
                        RaycastHit hitout;

                        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hitout, range, LayerMask.GetMask("Defense")))
                        {
                            placeLocation = hitout.point;
                            defenseGhost.transform.rotation = Quaternion.FromToRotation(defenseGhost.transform.forward, hitout.normal) * defenseGhost.transform.rotation;
                            validPlacement = true;
                        }
                    }

                    if (validPlacement)
                    {
                        ghostRenderer.SetMaterials(validMat);
                    }
                    else
                    {
                        ghostRenderer.SetMaterials(invalidMat);
                    }


                    if (player.CanAffordResources(defenses[activeDefense].diamondCost) && validPlacement)
                    {
                        ghostRenderer.SetMaterials(validMat);

                        if (Input.GetButtonDown("Fire1") && freeToPlay)
                        {
                            if (hitTowerWall != null)
                            {
                                dataManager.AddTowerPlacedOnWall(hitTowerWall);
                            }


                            Instantiate(defenses[activeDefense].defense, defenseGhost.transform.position, defenseGhost.transform.rotation);


                            player.ChangeDiamondAmount(-defenses[activeDefense].diamondCost);
                            player.miniGameScript.DisplayText(TUTORIAL_STEPS.PLACE_DEFENSE + 1);
                            latchAngle = 0;
                            if (activeDefense == 1)
                            {
                                player.miniGameScript.DisplayText(TUTORIAL_STEPS.PLACE_TURRET + 1);
                            }

                        }
                    }
                    else
                    {
                        ghostRenderer.SetMaterials(invalidMat);
                        if (Input.GetButtonDown("Fire1") && freeToPlay)
                        {
                            StartCoroutine(player.hudScript.CantAffordResourcesFlash());
                        }
                    }

                    #region
                    /*** OLD SYSTEM 

                    Vector3 point = playerCamera.transform.position + playerCamera.transform.TransformDirection(Vector3.forward) * range;
                    Vector3 girdLocationForDefense = Vector3.zero;

                    //if wall is active
                    if (activeDefense == 0)
                    {
                        if (rotateNum % 4 == 0)
                        {
                            girdLocationForDefense = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize + (gridSize / 2), 0, Mathf.RoundToInt(point.z / gridSize) * gridSize);
                        }
                        else if (rotateNum % 4 == 1)
                        {
                            girdLocationForDefense = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize, 0, Mathf.RoundToInt(point.z / gridSize) * gridSize + (gridSize / 2));
                        }
                        else if (rotateNum % 4 == 2)
                        {
                            girdLocationForDefense = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize - (gridSize / 2), 0, Mathf.RoundToInt(point.z / gridSize) * gridSize);
                        }
                        else if (rotateNum % 4 == 3)
                        {
                            girdLocationForDefense = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize, 0, Mathf.RoundToInt(point.z / gridSize) * gridSize - (gridSize / 2));
                        }
                    }
                    //if tower is active then set loc 
                    else if (activeDefense == 1)
                    {
                        girdLocationForDefense = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize, 0, Mathf.RoundToInt(point.z / gridSize) * gridSize);
                    }

                    //if ladder is active then set the location on grid
                    else if (activeDefense == 2)
                    {
                        RaycastHit hitout;

                        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hitout, range, LayerMask.GetMask("Defense")))
                        {
                            girdLocationForDefense = hitout.point;
                            defenseGhost.transform.rotation = Quaternion.FromToRotation(defenseGhost.transform.forward, hitout.normal) * defenseGhost.transform.rotation;
                        }
                    }

                    //set defense to the respective grid location
                    defenseGhost.transform.position = girdLocationForDefense;

                    if (!dataManager.DefenseAlreadyAtLoc(defenseGhost.transform.position))
                    {
                        if (player.CanAffordResources(defenses[activeDefense].woodCost, defenses[activeDefense].ironCost, defenses[activeDefense].diamondCost))
                        {
                            ghostRenderer.SetMaterials(validMat);

                            if (Input.GetButtonDown("Fire1") && freeToPlay)
                            {
                                Instantiate(defenses[activeDefense].defense, defenseGhost.transform.position, defenseGhost.transform.rotation);
                                dataManager.AddDefenseLocation(defenseGhost.transform.position);
                                player.SetResourceAmount(-defenses[activeDefense].woodCost, -defenses[activeDefense].ironCost, -defenses[activeDefense].diamondCost);

                            }
                        }
                        else
                        {
                            ghostRenderer.SetMaterials(invalidMat);
                            if (Input.GetButtonDown("Fire1") && freeToPlay)
                            {
                                StartCoroutine(player.hudScript.CantAffordResourcesFlash());
                            }
                        }

                    }
                    else
                    {
                        ghostRenderer.SetMaterials(invalidMat);

                    }

                    if (Input.GetButtonDown("RotateDefense"))
                    {

                        if (rotateNum % 2 == 0)
                        {
                            defenseGhost.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                        }
                        else
                        {
                            defenseGhost.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        }
                        rotateNum++;
                    }
                    ***/
                    #endregion
                    break;


                case PlayerMode.Tower:
                    #region TOWER FUNCTIONALITY
                    if (Input.GetKeyDown("1"))
                    {
                        ChangeTower(0);
                    }
                    if (Input.GetKeyDown("2"))
                    {
                        ChangeTower(1);
                    }
                    if (Input.GetKeyDown("3"))
                    {
                        ChangeTower(2);
                    }
                    if (Input.GetKeyDown("4"))
                    {
                        ChangeTower(3);
                    }

                    if (Input.GetButtonDown("Utility"))
                    {
                        EquipTowerUtility();
                        player.miniGameScript.DisplayText(TUTORIAL_STEPS.WRENCH + 1);
                        //equip utility
                    }



                    if (isTowerUtility)
                    {
                        if (Input.GetButtonDown("Fire1"))
                        {
                            GameObject hammer = Instantiate(towerWrenchThrowable, towerRoot.transform.position, towerRoot.transform.rotation);
                            hammer.GetComponent<WrenchThrowableScript>().SetPlayer(player);
                            player.miniGameScript.DisplayText(TUTORIAL_STEPS.THROW_WRENCH + 1);
                        }

                        return;
                    }

                    validPlacement = true;

                    RaycastHit hit;
                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, range, possibleLayers))
                    {
                        currentTower.transform.position = hit.point;
                        int objLayer = (1 << hit.collider.gameObject.layer);
                        if ((currentTowerScript.placementLayers.value & objLayer) <= 0)
                        {

                            validPlacement = false;
                        }

                        if (!player.CanAffordSoulFire(currentTowerScript.GetTowerCost()))
                        {
                            validPlacement = false;
                        }


                        if (currentTowerScript.GetIsSnapping())
                        {
                            if (!hit.collider.gameObject.CompareTag("SpawnPoint"))
                            {
                                currentTower.transform.position = hit.point;
                                validPlacement = false;
                            } else
                            {
                                currentTower.transform.position = hit.collider.transform.position;
                            }


                            if (snappableTowers.Contains(currentTower.transform.position))
                            {
                                validPlacement = false;
                            }

                            if (validPlacement)
                            {
                                if (Input.GetButtonDown("Fire1"))
                                {
                                    currentTower.transform.SetParent(hit.collider.transform);
                                    currentTowerScript.Place();
                                    snappableTowers.Add(currentTower.transform.position);
                                    player.SetSoulFire(-currentTowerScript.GetTowerCost());
                                    SpawnNewTower();
                                    player.miniGameScript.DisplayText(TUTORIAL_STEPS.PLACE_SENTRY + 1);
                                }
                            }



                        }
                        else
                        {
                            //HIGH HIGH PROBABILITY THAT THIS WILL FAIL WITH OTHER TOWERS CHECK THIS WITH FURNACE
                            if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Tower")))
                            {
                                validPlacement = false;
                            }


                            currentTower.transform.position = hit.point;

                            if (validPlacement)
                            {
                                if (Input.GetButtonDown("Fire1"))
                                {
                                    currentTowerScript.Place();
                                    SpawnNewTower();
                                    player.SetSoulFire(-currentTowerScript.GetTowerCost());
                                }
                            }

                        }
                    }
                    else
                    {
                        validPlacement = false;
                    }
                    print(validPlacement) ;

                    if (validPlacement)
                    {
                        currentTowerScript.SetMaterials(validMat);
                    }
                    else
                    {
                        currentTowerScript.SetMaterials(invalidMat);
                    }



                    #endregion
                    break;
                default:
                    break;
            }

        }



        protected override void CycleMode()
        {
            
            if (player.currentStep < TUTORIAL_STEPS.CYCLE_MODE)
            {
                return;
            }

            player.miniGameScript.DisplayText(TUTORIAL_STEPS.CYCLE_MODE + 1);
            
            switch (mode)
            {
                case PlayerMode.Weapons:
                    mode = PlayerMode.Defense;
                    player.miniGameScript.DisplayText(TUTORIAL_STEPS.BUILD_DEFENSES + 1);
                    StartBuilding();
                    break;
                case PlayerMode.Defense:
                    mode = PlayerMode.Tower;
                    player.miniGameScript.DisplayText(TUTORIAL_STEPS.TOWER_MODE + 1);
                    player.miniGameScript.DisplayText(TUTORIAL_STEPS.TOWER_MODE2 + 1);
                    StopBuilding();
                    EquipTower();
                    break;
                case PlayerMode.Tower:
                    mode = PlayerMode.Weapons;
                    EquipWeapons();
                    break;
            }
            player.hudScript.UpdatePlayerMode(mode);
        }

    }
}


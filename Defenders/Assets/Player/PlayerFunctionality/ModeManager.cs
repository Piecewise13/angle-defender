using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;


public class ModeManager : MonoBehaviour
{

    protected PlayerMode mode;
    protected PlayerDataMangerScript dataManager;
    protected PlayerScript player;
    public Animator playerAnimator;
    protected Camera playerCamera;

    protected bool freeToPlay = true;

    /*
     * WEAPON VARS
     */
    [Header("Weapons")]

    public GameObject[] equipedWeapons;
    protected WeaponInformation[] equipedWeaponInformation;
    protected int equipedWeaponIndex;
    public WeaponInformation basicPistol;
  
    public GameObject weaponRoot;
    protected ParentWeaponScript equipedWeapon;


    protected List<WeaponInformation>[] weaponOptions = new List<WeaponInformation>[3];

    [SerializeField] protected int maxWeaponsToHold = 4;

    /*
     * TOWER VARS
     */
    [Space(30)]
    [Header("Tower")]
    public GameObject[] towers;

    public GameObject towerWrenchPlayer;
    public GameObject towerWrenchThrowable;

    public GameObject towerPlacerObject;

    public GameObject towerRoot;
    protected bool isTowerUtility;
    protected int towerIndex;
    protected GameObject currentTower;
    //protected GameObject towerGhost;
    protected TowerParentScript currentTowerScript;
    public LayerMask possibleLayers;
    protected static List<Vector3> snappableTowers = new List<Vector3>();

    /*
     * BUILDING VARS
     */
    [Space(30)]
    [Header("Building")]
    public Defense[] defenses;
    protected int activeDefense;
    protected bool isDefenseUtility;
    public GameObject defenseUtilityObject;
    [SerializeField] protected float defenseUtilityRange;
    public LayerMask defenseLayer;


    public float defenseRotationSpeed;
    protected float latchAngle;
    protected Collider hitCollider = null;

    [SerializeField] protected float range;

    public GameObject defenseRoot;
    public float gridSize;


    protected GameObject defenseGhost;
    public Material validMat;
    public Material invalidMat;
    protected GhostScript ghostRenderer;
    protected Vector3 placeLocation = Vector3.up * -500;
    protected bool isLatched = false;
    protected GameObject hitTowerWall;



    

    // Start is called before the first frame update
    public virtual void Start()
    {
        dataManager = FindObjectOfType<PlayerDataMangerScript>();
        player = GetComponentInChildren<PlayerScript>();
        playerCamera = GetComponentInChildren<Camera>();
        equipedWeaponInformation = new WeaponInformation[equipedWeapons.Length];

        for (int i = 0; i < weaponOptions.Length; i++)
        {
            weaponOptions[i] = new List<WeaponInformation>();
        }
        EquipNewGun(basicPistol);
        EquipWeapons();
    }

    // Update is called once per frame
    public virtual void Update()
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
                    } else
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
                        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit1 ,defenseUtilityRange, defenseLayer))
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
                        if (!isLatched) {
                            if (hitout.collider.CompareTag("Ground"))
                            {
                                placeLocation = hitout.point;
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
                                    placeLocation = location;
                                }
                                else
                                {
                                    WallDefenceScript.showIndicators = true;
                                    isLatched = false;
                                }
                                
                                defenseGhost.transform.LookAt(hitCollider.transform.position.xz3(), Vector3.up);

                            }
                            else
                            {
                                defenseGhost.transform.Rotate(Vector3.up * (defenseRotationSpeed * Time.deltaTime));
                            }
                        } else
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
                        } else
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
                                placeLocation = hitout.point;
                                validPlacement = true;
                            }
                            else if (hitout.collider.CompareTag("Wall"))
                            {
                                isLatched = true;
                                placeLocation = (hitout.collider.transform.position + hitout.collider.transform.forward.normalized * 4).xz3();
                                hitCollider = hitout.collider;
                                validPlacement = true;
                            }
                        }

                        if (Input.GetButton("RotateDefense"))
                        {
                            if (isLatched)
                            {
                                RaycastHit rotHit;
                                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rotHit, 100f, LayerMask.GetMask("Ground")))
                                {
                                    print("rotating defense");
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
                                print("rotate defense");
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
                } else
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
                        latchAngle = 0;
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
                    //equip utility
                }



                if (isTowerUtility)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        GameObject hammer = Instantiate(towerWrenchThrowable, towerRoot.transform.position, towerRoot.transform.rotation);
                        hammer.GetComponent<WrenchThrowableScript>().SetPlayer(player);
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
                        return;
                    }

                    if (!player.CanAffordSoulFire(currentTowerScript.GetTowerCost()))
                    {
                        validPlacement = false;
                        return;
                    }

                    if (currentTowerScript.GetIsSnapping())
                    {
                        if (!hit.collider.gameObject.CompareTag("SpawnPoint"))
                        {
                            return;
                        }
                        currentTower.transform.position = hit.collider.transform.position;

                        if (snappableTowers.Contains(currentTower.transform.position))
                        {
                            validPlacement = false;
                            return;
                        }

                        if (Input.GetButtonDown("Fire1"))
                        {
                            currentTower.transform.SetParent(hit.collider.transform);
                            currentTowerScript.Place();
                            snappableTowers.Add(currentTower.transform.position);
                            player.SetSoulFire(-currentTowerScript.GetTowerCost());
                            SpawnNewTower();
                            print("placing at snap");
                        }


                    }
                    else
                    {
                        //HIGH HIGH PROBABILITY THAT THIS WILL FAIL WITH OTHER TOWERS CHECK THIS WITH FURNACE
                        if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Tower")))
                        {
                            validPlacement = false;
                            return;
                        }

                        currentTower.transform.position = hit.point;
                        if (Input.GetButtonDown("Fire1"))
                        {
                            currentTowerScript.Place();
                            SpawnNewTower();
                            player.SetSoulFire(-currentTowerScript.GetTowerCost());
                        }
                    }
                }
                else
                {
                    validPlacement = false;
                }

                if (validPlacement)
                {
                    currentTowerScript.SetMaterials(validMat);
                } else
                {
                    currentTowerScript.SetMaterials(invalidMat);
                }



                #endregion
                break;
            default:
                break;
        }

    }
    #region BUILDING METHODS

    protected void StartBuilding()
    {

        weaponRoot.SetActive(false);
        towerRoot.SetActive(false);
        defenseRoot.SetActive(true);
        defenseUtilityObject.SetActive(false);

        activeDefense = 0;
        CreateDefenseGhost();
        isLatched = false;
        hitCollider = null;

    }

    protected void StopBuilding()
    {

        weaponRoot.SetActive(true);
        towerRoot.SetActive(false);
        defenseRoot.SetActive(false);
        isDefenseUtility = false;
        Destroy(defenseGhost);
        player.hudScript.StopDisplayingHint();
    }

    void ChangeDefense(int index)
    {
        activeDefense = index;
        Destroy(defenseGhost);
        CreateDefenseGhost();

    }

    protected void CreateDefenseGhost()
    {
        defenseGhost = Instantiate(defenses[activeDefense].ghost);
        ghostRenderer = defenseGhost.GetComponent<GhostScript>();
        player.hudScript.PlacingEntity(false, defenses[activeDefense].diamondCost);
        player.hudScript.DisplayHint(PLAYER_HINT.COST);
    }

    protected void EquipBuildingUtility()
    {
        isDefenseUtility = true;
        defenseUtilityObject.SetActive(true);
        Destroy(defenseGhost);
    }

    protected void UnequipBuildingUtility()
    {
        isDefenseUtility = false;
        defenseUtilityObject.SetActive(false);
        StartBuilding();
    }

    #endregion

    #region WEAPONS METHOD
    public void EquipWeapons()
    {
        player.ChangeCameraZoom(1f);
        towerRoot.SetActive(false);
        weaponRoot.SetActive(true);
        Destroy(currentTower);
        player.hudScript.StopDisplayingHint();
        ChangeGun(0);
    }

    void ChangeGun(int index)
    {
        if (equipedWeapons[index] == null)
        {
            return;
        }
        foreach (GameObject weapon in equipedWeapons)
        {
            if (weapon != null)
                weapon.SetActive(false);
        }
        equipedWeaponIndex = index;
        equipedWeapons[equipedWeaponIndex].SetActive(true);
        player.hudScript.UpdateEquipedWeapon(equipedWeaponInformation[index].tier);

        try
        {
            equipedWeapon = equipedWeapons[equipedWeaponIndex].GetComponent<ParentWeaponScript>();
            playerAnimator.runtimeAnimatorController = equipedWeapon.gunAnims;
            equipedWeapon.EquipGun();
            player.hudScript.UpdateClipSize(equipedWeapon.clipSize);
        }
        catch
        {

        }

        playerAnimator.SetTrigger("newGun");
    }

   
    public bool GiveNewGun(WeaponInformation weapon)
    {
        int tier = weapon.tier;
        print(tier);
        print(weaponOptions.Length);
        if (weaponOptions[tier - 1].Count == maxWeaponsToHold)
        {
            return false;
        }

        weaponOptions[tier - 1].Add(weapon);
        return true;
    }


   

    public void EquipNewGun(WeaponInformation weapon)
    {
        equipedWeaponIndex = weapon.tier - 1;
        Destroy(equipedWeapons[equipedWeaponIndex]);
        equipedWeapons[equipedWeaponIndex] = Instantiate(weapon.weapon, weaponRoot.transform);
        equipedWeapons[equipedWeaponIndex].transform.localPosition = Vector3.zero;
        equipedWeaponInformation[equipedWeaponIndex] = weapon;

        player.hudScript.UpdateWeaponIcon(weapon.icon, weapon.tier);

        ChangeGun(equipedWeaponIndex);
    }
    
    public WeaponInformation GetEquipedWeaponInformation(int tier)
    {
        return equipedWeaponInformation[tier - 1];
    } 

    //ABSOLUTLEY HATE THIS MORE THEN TRYING TO SPELL ABSOULTELY
    public List<WeaponInformation> GetWeaponOptions(int tier)
    {
        WeaponInformation equiped = equipedWeaponInformation[tier - 1];
        List<WeaponInformation> options = new List<WeaponInformation>();
        foreach (var option in weaponOptions[tier - 1])
        {
            if (!option.Equals(equiped))
            {
                options.Add(option);
            }
        }

        return options;
    }

    #endregion

    #region TOWER METHODS
    public void EquipTower()
    {
        player.ChangeCameraZoom(.8f);
        player.animator.SetBool("isAiming", false);
        player.lookScript.bUseAimSens = false;
        weaponRoot.SetActive(false);
        towerRoot.SetActive(true);
        foreach (var item in equipedWeapons)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }

        ChangeTower(0);
        
    }

    void SpawnNewTower()
    {
        currentTower = Instantiate(towers[towerIndex]);
        currentTowerScript = currentTower.GetComponent<TowerParentScript>();
        currentTowerScript.SetMaterials(invalidMat);
        player.hudScript.PlacingEntity(true, currentTowerScript.GetTowerCost());
        player.hudScript.DisplayHint(PLAYER_HINT.COST);
    }

    void ChangeTower(int index)
    {
        isTowerUtility = false;
        towerWrenchPlayer.SetActive(false);
        towerPlacerObject.SetActive(true);
        towerIndex = index;
        if(currentTower != null)
        {
            Destroy(currentTower);
        }

        SpawnNewTower();
        
        //TODO make a cool animation and play it here


    }

    void EquipTowerUtility()
    {
        Destroy(currentTower);
        towerWrenchPlayer.SetActive(true);
        towerPlacerObject.SetActive(false);
        isTowerUtility = true;
    }


    public bool TowerThrown()
    {
        return true;
        
    }

    public void TowerRemoved(GameObject tower)
    {
        snappableTowers.Remove(tower.transform.position);
    }
    #endregion

    protected void CycleMode()
    {
        switch (mode)
        {
            case PlayerMode.Weapons:
                mode = PlayerMode.Defense;
                StartBuilding();
                break;
            case PlayerMode.Defense:
                mode = PlayerMode.Tower;
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

    public void canShoot(bool value)
    {
        ParentWeaponScript.SetCanShoot(value);
    }

    public void SetFreeToPlay(bool value)
    {
        freeToPlay = value;
        canShoot(value);
    }

    public ParentWeaponScript GetEquipedWeapon()
    {
        return equipedWeapon;
    }

    public PlayerMode GetPlayerMode()
    {
        return mode;
    }


}

[System.Serializable]
public class Defense
{
    public GameObject ghost;
    public GameObject defense;

    public int diamondCost;

}

public enum PlayerMode
{
    Weapons,
    Defense,
    Tower
}
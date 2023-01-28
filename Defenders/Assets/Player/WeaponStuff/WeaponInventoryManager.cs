using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryManager : MonoBehaviour
{

    private PlayerMode mode;

    private PlayerScript player;
    public Animator playerAnimator;
    private Camera playerCamera;

    private bool freeToPlay = true;

    /*
     * WEAPON VARS
     */
    [Header("Weapons")]
    public GameObject[] weapons;
    private int weaponIndex;

    public int unlockedTier = 0;
  
    public GameObject weaponRoot;
    private BasicWeaponScript equipedWeapon;

    /*
     * TOWER VARS
     */
    [Space(30)]
    [Header("Tower")]
    public GameObject[] towers;

    public GameObject towerHammerPlayer;
    public GameObject towerHammerThrow;

    public GameObject towerPlacerObject;

    public GameObject towerRoot;
    private bool isTowerUtility;
    private int towerIndex;
    private GameObject currentTower;
    //private GameObject towerGhost;
    private TowerParentScript currentTowerScript;
    public LayerMask possibleLayers;
    private static List<Vector3> snappableTowers = new List<Vector3>();

    /*
     * BUILDING VARS
     */
    [Space(30)]
    [Header("Building")]
    public Defense[] defenses;
    private int activeDefense;
    private bool isRemoving;

    private int rotateNum;
    [SerializeField] private float range;

    public GameObject defenseRoot;
    public float gridSize;


    private GameObject defenseGhost;
    public Material validMat;
    public Material invalidMat;
    private GhostScript ghostRenderer;

    public LayerMask removeLayer;


    List<Vector3> defenseLocations = new List<Vector3>();





    //public float damageMultiplier;




    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<PlayerScript>();
        playerCamera = GetComponentInChildren<Camera>();
        EquipWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchMode"))
        {
            CycleMode();
        }

        if (!freeToPlay)
        {
            return;
        }

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
            case PlayerMode.Building:
                if (Input.GetKeyDown("1"))
                {
                    ChangeDefense(0);
                }
                if (Input.GetKeyDown("2"))
                {
                    ChangeDefense(1);
                }
                if (Input.GetKeyDown("3"))
                {
                    ChangeDefense(2);
                }
                if (Input.GetKeyDown("4"))
                {
                    ChangeDefense(3);
                }

                if (Input.GetButtonDown("Remover"))
                {
                    isRemoving = !isRemoving;
                    if (isRemoving)
                    {
                        Destroy(defenseGhost);
                    }
                    else
                    {
                        StartBuilding();
                    }
                }

                if (!isRemoving)
                {
                    Vector3 point = playerCamera.transform.position + playerCamera.transform.TransformDirection(Vector3.forward) * range;
                    Vector3 loc = Vector3.zero;
                    if (activeDefense == 0)
                    {
                        if (rotateNum % 4 == 0)
                        {
                            loc = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize + (gridSize / 2), 0, Mathf.RoundToInt(point.z / gridSize) * gridSize);
                        }
                        else if (rotateNum % 4 == 1)
                        {
                            loc = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize, 0, Mathf.RoundToInt(point.z / gridSize) * gridSize + (gridSize / 2));
                        }
                        else if (rotateNum % 4 == 2)
                        {
                            loc = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize - (gridSize / 2), 0, Mathf.RoundToInt(point.z / gridSize) * gridSize);
                        }
                        else if (rotateNum % 4 == 3)
                        {
                            loc = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize, 0, Mathf.RoundToInt(point.z / gridSize) * gridSize - (gridSize / 2));
                        }
                    }
                    else if (activeDefense == 1)
                    {
                        loc = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize, 0, Mathf.RoundToInt(point.z / gridSize) * gridSize);
                    }
                    else if (activeDefense == 2)
                    {
                        RaycastHit hitout;

                        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hitout, range, 512))
                        {
                            loc = hitout.point;
                            defenseGhost.transform.rotation = Quaternion.FromToRotation(defenseGhost.transform.forward, hitout.normal) * defenseGhost.transform.rotation;
                        }
                    }

                    defenseGhost.transform.position = loc;

                    if (!defenseLocations.Contains(defenseGhost.transform.position))
                    {
                        //check if player can afford

                        if (player.CanAffordResources(defenses[activeDefense].woodCost, defenses[activeDefense].ironCost, defenses[activeDefense].diamondCost))
                        {
                            ghostRenderer.SetMaterials(validMat);

                            if (Input.GetButtonDown("Fire1") && freeToPlay)
                            {
                                Instantiate(defenses[activeDefense].defense, defenseGhost.transform.position, defenseGhost.transform.rotation);
                                defenseLocations.Add(defenseGhost.transform.position);
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
                }
                else
                {
                    RaycastHit hitray;
                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hitray, range, removeLayer))
                    {
                        if (Input.GetButtonDown("Fire1") && freeToPlay)
                        {
                            Destroy(hitray.collider.transform.root.gameObject);
                        }
                    }
                }
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
                        GameObject hammer = Instantiate(towerHammerThrow, towerRoot.transform.position, towerRoot.transform.rotation);
                        hammer.GetComponent<HammerThrowableScript>().SetPlayer(player);
                    }

                    return;
                }

                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, range, possibleLayers))
                {
                    currentTower.transform.position = hit.point;
                    int objLayer = (1 << hit.collider.gameObject.layer);
                    if ((currentTowerScript.placementLayers.value & objLayer) <= 0)
                    {
                        currentTowerScript.SetMaterials(invalidMat);
                        return;
                    }

                    if (!player.CanAffordSoulFire(currentTowerScript.GetTowerCost()))
                    {
                        currentTowerScript.SetMaterials(invalidMat);
                        return;
                    }


                    currentTowerScript.SetMaterials(validMat);
                    if (currentTowerScript.GetIsSnapping())
                    {
                        currentTower.transform.position = hit.collider.transform.position;

                        if (snappableTowers.Contains(currentTower.transform.position))
                        {
                            currentTowerScript.SetMaterials(invalidMat);
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
                            currentTowerScript.SetMaterials(invalidMat);
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
                    currentTowerScript.SetMaterials(invalidMat);
                }
                #endregion
                break;
            default:
                break;
        }

    }
    #region BUILDING METHODS

    private void StartBuilding()
    {
        weaponRoot.SetActive(false);
        towerRoot.SetActive(false);
        defenseRoot.SetActive(true);
        activeDefense = 0;
        defenseGhost = Instantiate(defenses[activeDefense].ghost);
        
        ghostRenderer = defenseGhost.GetComponent<GhostScript>();
        rotateNum = 0;
        
    }

    private void StopBuilding()
    {

        weaponRoot.SetActive(true);
        towerRoot.SetActive(false);
        defenseRoot.SetActive(false);
        Destroy(defenseGhost);
    }
    void ChangeDefense(int index)
    {
        activeDefense = index;
        Destroy(defenseGhost);
        defenseGhost = Instantiate(defenses[activeDefense].ghost);
        ghostRenderer = defenseGhost.GetComponent<GhostScript>();
        print("change tower");
    }

    #endregion

    #region WEAPONS METHOD
    public void EquipWeapons()
    {
        player.ChangeCameraZoom(1f);
        towerRoot.SetActive(false);
        weaponRoot.SetActive(true);
        Destroy(currentTower);

        ChangeGun(0);
    }

    void ChangeGun(int index)
    {
        if (weapons[index] == null)
        {
            return;
        }
        foreach (GameObject weapon in weapons)
        {
            if (weapon != null)
                weapon.SetActive(false);
        }
        weaponIndex = index;
        weapons[weaponIndex].SetActive(true);



        try
        {
            equipedWeapon = weapons[weaponIndex].GetComponent<BasicWeaponScript>();
            player.hudScript.UpdateEquipedWeapon(index + 1);
            playerAnimator.runtimeAnimatorController = equipedWeapon.gunAnims;
            equipedWeapon.EquipGun();
            //equipedWeapon.SetPlayer(player);
            //BasicWeaponScript.damageMultiplier = damageMultiplier;
        }
        catch
        {

        }

        playerAnimator.SetTrigger("newGun");
    }

    public void GiveNewGun(GameObject gun, int weaponTier)
    {
        //print(gun);
        weaponIndex = weaponTier - 1;
        Destroy(weapons[weaponIndex]);
        weapons[weaponIndex] = Instantiate(gun, weaponRoot.transform);
        weapons[weaponIndex].transform.localPosition = Vector3.zero;
        BasicWeaponScript script = gun.GetComponent<BasicWeaponScript>();

        player.hudScript.UpdateWeaponIcon(script.weaponIcon, weaponTier);

        //print("length: " + weapons.Length);
        ChangeGun(weaponIndex);
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
        foreach (var item in weapons)
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
    }

    void ChangeTower(int index)
    {
        isTowerUtility = false;
        towerHammerPlayer.SetActive(false);
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
        towerHammerPlayer.SetActive(true);
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

    private void CycleMode()
    {
        switch (mode)
        {
            case PlayerMode.Weapons:
                mode = PlayerMode.Building;
                StartBuilding();
                break;
            case PlayerMode.Building:
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
        BasicWeaponScript.SetCanShoot(value);
    }

    public void SetFreeToPlay(bool value)
    {
        freeToPlay = value;
        canShoot(value);
    }


}

[System.Serializable]
public class Defense
{
    public GameObject ghost;
    public GameObject defense;

    public int woodCost, ironCost, diamondCost;

}

public enum PlayerMode
{
    Weapons,
    Building,
    Tower
}
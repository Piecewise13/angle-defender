using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryManager : MonoBehaviour
{

    private PlayerMode mode;

    private PlayerScript player;
    public Animator playerAnimator;
    private Camera playerCamera;
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
    public TowerHolder[] towers;
    public GameObject towerRoot;
    private int towerIndex;
    private GameObject towerGhost;
    private GhostScript towerRenderer;
    public LayerMask possibleLayers;

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

                            if (Input.GetButtonDown("Fire1"))
                            {
                                Instantiate(defenses[activeDefense].defense, defenseGhost.transform.position, defenseGhost.transform.rotation);
                                defenseLocations.Add(defenseGhost.transform.position);
                                player.SetResourceAmount(-defenses[activeDefense].woodCost, -defenses[activeDefense].ironCost, -defenses[activeDefense].diamondCost);

                            }
                        }
                        else
                        {
                            ghostRenderer.SetMaterials(invalidMat);
                            if (Input.GetButtonDown("Fire1"))
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
                        if (Input.GetButtonDown("Fire1"))
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

                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, range, possibleLayers))
                {
                    towerGhost.transform.position = hit.point;
                    int objLayer = (1 << hit.collider.gameObject.layer);
                    if ((towers[towerIndex].placeLayers.value & objLayer) > 0)
                    {
                        towerRenderer.SetMaterials(validMat);
                        if (towers[towerIndex].isSnapping)
                        {
                            towerGhost.transform.position = hit.collider.transform.position;
                            if (Input.GetButtonDown("Fire1"))
                            {
                                Instantiate(towers[towerIndex].tower, towerGhost.transform.position, towerGhost.transform.rotation);
                                towers[towerIndex].numHeld--;
                                print("placing at snap");
                                CleanUpTowerArray();
                            }
                        }
                        else
                        {
                            towerGhost.transform.position = hit.point;
                            if (Input.GetButtonDown("Fire1"))
                            {
                                Instantiate(towers[towerIndex].tower, hit.point, towerGhost.transform.rotation);
                                towers[towerIndex].numHeld--;
                                CleanUpTowerArray();
                            }
                        }

                    }
                    else
                    {
                        towerRenderer.SetMaterials(invalidMat);
                    }
                }
                else
                {
                    towerRenderer.SetMaterials(invalidMat);
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
        Destroy(towerGhost);

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
        if (towers[0] != null && towers[0].numHeld != 0)
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
        else
        {
            CycleMode();
        }
        
    }

    void ChangeTower(int index)
    {
        if (towers[index] == null || towers[index].numHeld == 0)
        {
            return;
        }

        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] != null)
            {
                if (towers[i].numHeld != 0)
                {
                    Destroy(towerGhost);
                    towerGhost = Instantiate(towers[index].ghost, Vector3.zero, Quaternion.Euler(Vector3.zero));
                    towerRenderer = towerGhost.GetComponentInChildren<GhostScript>();
                }

            }
        }
        towerIndex = index;


        playerAnimator.SetTrigger("newGun");

        /*
         * OLD METHOD

        */

    }

    public bool GiveNewTower(TowerHolder tower)
    {

        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] != null && towers[i].numHeld != 0)
            {
                //this most likely wont work but maybe
                if (towers[i].Equals(tower))
                {
                    print("Already have one");
                    towers[i].numHeld++;
                    //EquipTower();
                    //ChangeTower(i);
                    return true;
                }
            }

        }
        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] == null || towers[i].numHeld == 0)
            {
                print("made a new one");
                towers[i] = tower;
                towers[i].numHeld = 1;

                //EquipTower();
                //ChangeTower(i);
                return true;
            }
        }

        return false;

    }





    private void CleanUpTowerArray()
    {

        int lastAval = -1;
        for (int i = 0; i < towers.Length - 1; i++)
        {

            if (towers[i] == null || towers[i].numHeld == 0)
            {

                if (towers[i+1] == null)
                {
                    print("List should be sorted");
                    break;
                }

                towers[i] = towers[i + 1];
                towers[i + 1] = null;
                    
            } else
            {
                print("setting last avalible to " + i);
                lastAval = i;
            }
        }

        if (lastAval == -1)
        {
            if (towers[0] == null || towers[0].numHeld == 0)
            {
                print("no more towers");
                EquipWeapons();
            } else
            {
                ChangeTower(0);
                print("this is the exeception");
            }

        } else
        {
            ChangeTower(lastAval);
            print("Switching to tower: " + lastAval);
        }

    }

    public bool TowerThrown()
    {
        return true;
        /*
        if (towers[towerIndex].numHeld != 0)
        {
            towers[towerIndex].numHeld--;
            
            if (towers[towerIndex].numHeld == 0)
            {
                print("runnin this hsit");
                Destroy(towers[towerIndex].towerObject);
                CleanUpTowerArray();
            }
            return true;
        } else
        {
            return false;
        }
        */
        
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
        //print(mode);
    }

    public void canShoot(bool value)
    {
        BasicWeaponScript.SetCanShoot(value);
    }


}

[System.Serializable]
public class TowerHolder
{

    public GameObject ghost;
    public GameObject tower;
    [HideInInspector]public int numHeld;
    public LayerMask placeLayers;
    public bool isSnapping;


    public bool Equals(TowerHolder obj)
    {
        return tower = obj.tower;
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
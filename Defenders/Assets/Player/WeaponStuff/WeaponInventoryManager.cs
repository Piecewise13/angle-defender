using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryManager : MonoBehaviour
{

    private bool weaponsActive = true;

    public GameObject[] weapons;
    public GameObject weaponRoot;
    private BasicWeaponScript equipedWeapon;

    public TowerHolder[] towers;
    public GameObject towerRoot;
    private int towerIndex;

    [Space(30)]
    [Header("Building")]
    public Defense[] defenses;
    private int activeDefense;

    private bool isBuilding;
    private bool isRemoving;

    private int rotateNum;
    [SerializeField] private float range;

    public GameObject defenseRoot;
    public float gridSize;


    private GameObject defenseGhost;
    public Material validMat;
    public Material invalidMat;
    private Renderer ghostRenderer;

    public LayerMask removeLayer;


    List<Vector3> defenseLocations = new List<Vector3>();




    private PlayerScript player;
    private int weaponIndex;

    public int unlockedTier = 0;
    //public float damageMultiplier;

    public Animator playerAnimator;

    private BuildingScript buildingScript;
    private Camera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
        buildingScript = GetComponent<BuildingScript>();
        player = GetComponentInChildren<PlayerScript>();
        playerCamera = GetComponentInChildren<Camera>();
        EquipWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("BuildingToggle"))
        {
            if (isBuilding)
            {
                StopBuilding();
            }
            else
            {
                StartBuilding();
            }
        }


        if (!isBuilding)
        {
            if (Input.GetButtonDown("WeaponToggle"))
            {
                weaponsActive = !weaponsActive;
                if (weaponsActive)
                {
                    EquipWeapons();

                }
                else
                {
                    EquipTower();

                }
            }

            if (weaponsActive)
            {
                if (Input.GetKeyDown("1"))
                {
                    ChangeGun(0);

                }
                if (Input.GetKeyDown("2"))
                {
                    ChangeGun(1);
                }



            }
            else
            {
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

            }
        }
        else
        {


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
                }  else
                {

                }


            } else
            {
                StartBuilding();
            }


            if (!isRemoving)
            {
                Vector3 point = playerCamera.transform.position + playerCamera.transform.TransformDirection(Vector3.forward) * range;
                Vector3 loc = Vector3.zero;
                if (activeDefense == 0)
                {
                    if (rotateNum % 2 == 0)
                    {
                        loc = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize, 0, Mathf.RoundToInt(point.z / gridSize) * gridSize);
                    }
                    else
                    {
                        loc = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize + (gridSize / 2), 0, Mathf.RoundToInt(point.z / gridSize) * gridSize + (gridSize / 2));
                    }
                }
                else if (activeDefense == 1)
                {
                    loc = new Vector3(Mathf.RoundToInt(point.x / gridSize) * gridSize + (gridSize / 2), 0, Mathf.RoundToInt(point.z / gridSize) * gridSize);
                }

                defenseGhost.transform.position = loc;

                if (!defenseLocations.Contains(defenseGhost.transform.position))
                {
                    ghostRenderer.material = validMat;
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Instantiate(defenses[activeDefense].defense, defenseGhost.transform.position, defenseGhost.transform.rotation);
                        defenseLocations.Add(defenseGhost.transform.position);

                    }
                }
                else
                {
                    ghostRenderer.material = invalidMat;

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
            } else
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, range,  removeLayer))
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Destroy(hit.collider.transform.root.gameObject);
                    }
                }
            }
        }
    }

    private void StartBuilding()
    {
        isBuilding = true;
        weaponRoot.SetActive(false);
        towerRoot.SetActive(false);
        defenseRoot.SetActive(true);
        defenseGhost = Instantiate(defenses[0].ghost, Vector3.zero, Quaternion.Euler(Vector3.zero));
        ghostRenderer = defenseGhost.GetComponentInChildren<Renderer>();
        
    }

    private void StopBuilding()
    {
        isBuilding = false;
        weaponRoot.SetActive(true);
        towerRoot.SetActive(false);
        defenseRoot.SetActive(false);
        weaponsActive = true;
        Destroy(defenseGhost);
    }


    public void EquipTower()
    {
        if (towers[0] != null && towers[0].numHeld!= 0)
        {
            weaponsActive = false;
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
        } else
        {
            weaponsActive = true;
        }

    }

    public void EquipWeapons()
    {
        weaponsActive = true;
        player.ChangeCameraZoom(1f);
        towerRoot.SetActive(false);
        weaponRoot.SetActive(true);
        int index = -1;

        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] != null && towers[i].numHeld != 0)
            {
                if (index == -1)
                {

                }
                towers[i].towerObject.SetActive(false);
            }
        }

        ChangeGun(0);
    }


    void ChangeTower(int index)
    {

        if (towers[index] == null ||towers[index].numHeld == 0)
        {
            return;
        }

        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] != null)
            {
                if (towers[i].numHeld != 0)
                {
                    towers[i].towerObject.SetActive(false);
                    towers[i].towerScript.canFire = false;
                    towers[index].towerScript.playerInventory = this;
                }

            }
        }
        towerIndex = index;
        towers[index].towerObject.SetActive(true);
        towers[index].towerScript.canFire = true;


        playerAnimator.SetTrigger("newGun");

    }



    void ChangeGun(int index)
    {
        if (weapons[index] == null)
        {
            return;
        }
        foreach (GameObject weapon in weapons)
        {
            if(weapon != null)
                weapon.SetActive(false);
        }
        weaponIndex = index;
        weapons[weaponIndex].SetActive(true);
        
        

        try{
            equipedWeapon = weapons[weaponIndex].GetComponent<BasicWeaponScript>();
            player.hudScript.UpdateEquipedWeapon(index + 1);
            playerAnimator.runtimeAnimatorController = equipedWeapon.gunAnims;
            equipedWeapon.EquipGun();
            //equipedWeapon.SetPlayer(player);
            //BasicWeaponScript.damageMultiplier = damageMultiplier;
        } catch
        {

        }

        playerAnimator.SetTrigger("newGun");
    }

    void ChangeDefense(int index)
    {
        activeDefense = index;
        Destroy(defenseGhost);
        defenseGhost = Instantiate(defenses[activeDefense].ghost);
        ghostRenderer = defenseGhost.GetComponentInChildren<Renderer>();
    }



    public bool GiveNewTower(GameObject tower)
    {
        ThrowableTowerScript script = tower.GetComponent<ThrowableTowerScript>();

        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] != null && towers[i].numHeld != 0)
            {
                //this most likely wont work but maybe
                if (towers[i].towerScript.towerName.Equals(script.towerName))
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
                towers[i] = new TowerHolder(Instantiate(tower, towerRoot.transform), 1);
                towers[i].numHeld = 1;
                
                //EquipTower();
                //ChangeTower(i);
                return true;
            }
        }

        return false;
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
                print("nmo more towers");
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

        
    }
    

    public void canShoot(bool value)
    {
        weapons[weaponIndex].GetComponent<BasicWeaponScript>().setCanShoot(value);
    }


}

[System.Serializable]
public class TowerHolder
{

    public ThrowableTowerScript towerScript;
    public GameObject towerObject;
    public int numHeld;

    public TowerHolder(GameObject tower, int num)
    {
        this.towerObject = tower;
        towerScript = towerObject.GetComponent<ThrowableTowerScript>();
        this.numHeld = num;
    }


}

[System.Serializable]
public class Defense
{
    public GameObject ghost;
    public GameObject defense;

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryManager : MonoBehaviour
{

    public GameObject[] weapons;
    public GameObject weaponRoot;
    private BasicWeaponScript equipedWeapon;




    private PlayerScript player;
    private int weaponIndex;

    public int unlockedTier = 0;
    //public float damageMultiplier;

    public Animator playerAnimator;

    private BuildingScript buildingScript;

    // Start is called before the first frame update
    void Start()
    {
        buildingScript = GetComponent<BuildingScript>();
        player = GetComponentInChildren<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            changeGun(0);

        }
        if (Input.GetKeyDown("2"))
        {
            changeGun(1);
        } 
    }

    void changeGun(int index)
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
            playerAnimator.runtimeAnimatorController = equipedWeapon.gunAnims;
            equipedWeapon.EquipGun();
            //equipedWeapon.SetPlayer(player);
            //BasicWeaponScript.damageMultiplier = damageMultiplier;
        } catch
        {

        }

        playerAnimator.SetTrigger("newGun");
    }

    public void canShoot(bool value)
    {
        weapons[weaponIndex].GetComponent<BasicWeaponScript>().setCanShoot(value);
    }




    public void GiveNewGun(GameObject gun, int weaponTier)
    {
        //print(gun);
        weaponIndex = weaponTier - 1;
        Destroy(weapons[weaponIndex]);
        weapons[weaponIndex] = Instantiate(gun, weaponRoot.transform);
        weapons[weaponIndex].transform.localPosition = Vector3.zero;

        //print("length: " + weapons.Length);
        changeGun(weaponIndex);
    }


}

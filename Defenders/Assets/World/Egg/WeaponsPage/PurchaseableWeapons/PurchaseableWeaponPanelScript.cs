using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseableWeaponPanelScript : MonoBehaviour
{
    public GridLayoutGroup grid;

    /*FOR TESTING ONLY*/
    [Tooltip("TESTING VAR")]
    [SerializeField] private List<WeaponInformation> weaponsAvailble;

    public EquipableWeaponPanelScript equipableWeapons;

    private EggShopScript eggShop;

    public GameObject purchaseableWidgetPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //grid = GetComponentInChildren<GridLayout>();
        eggShop = GetComponentInParent<EggShopScript>();
        BuildGrid(weaponsAvailble);
    }

    public bool PurchaseWeapon(WeaponInformation weapon)
    {
        PlayerScript player = eggShop.GetPlayer();
        if (!player.CanAffordSoulFire(weapon.cost))
        {
            return false;
        }
        player.SetSoulFire(-weapon.cost);
        return eggShop.GetPlayer().weaponManager.GiveNewGun(weapon);

    }

    public void BuildGrid(in List<WeaponInformation> weaponsToAdd)
    {
        foreach (var weapon in weaponsToAdd)
        {
            CreateWidget(weapon);
        }
    }

    private void CreateWidget(WeaponInformation info)
    {
        GameObject widget = Instantiate(purchaseableWidgetPrefab, grid.transform);
        PurchaseableWeaponWidget script = widget.GetComponent<PurchaseableWeaponWidget>();
        script.InitializeWidget(info);
        
    }
}

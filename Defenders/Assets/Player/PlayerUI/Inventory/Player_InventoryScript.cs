using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_InventoryScript : MonoBehaviour
{
    public RectTransform selectionPanel;
    private WeaponSelectionPanelScript selectionPanelScript;

    public RectTransform selectedIndicator;


    public RectTransform[] equipedWeaponsTransforms;
    private Image[] equipedWeaponsImages;

    private WeaponInventoryManager inventoryManager;


    // Start is called before the first frame update
    void Start()
    {
        selectionPanelScript = selectionPanel.GetComponent<WeaponSelectionPanelScript>();
        selectionPanel.gameObject.SetActive(false);
        
        equipedWeaponsImages = new Image[equipedWeaponsTransforms.Length];
        for (int i = 0; i < equipedWeaponsImages.Length; i++)
        {
            equipedWeaponsImages[i] = equipedWeaponsTransforms[i].GetComponent<Image>();
        }
        inventoryManager = GetComponentInParent<WeaponInventoryManager>();
    }


    public void SelectWeapon(int tier)
    {
        selectionPanel.gameObject.SetActive(true);
        selectionPanel.position = equipedWeaponsTransforms[tier - 1].position;
        selectionPanelScript.NewSelection(inventoryManager.GetWeaponOptions(tier));
    }


    public void ChangeWeapon(WeaponInformation newWeapon)
    {
        int tier = newWeapon.tier;

        equipedWeaponsImages[tier - 1].sprite = newWeapon.icon;
        inventoryManager.EquipNewGun(newWeapon);
        selectionPanelScript.NewSelection(inventoryManager.GetWeaponOptions(tier));
    }
}

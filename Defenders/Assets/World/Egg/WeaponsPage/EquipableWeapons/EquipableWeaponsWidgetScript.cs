using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipableWeaponsWidgetScript : MonoBehaviour
{
    [SerializeField] public static Color tier1Color = new Color(0.2941177f, 0.254902f, 0.7176471f);
    [SerializeField] public static Color tier2Color = new Color(0.5019608f, 0.2039216f, 0.6941177f);
    [SerializeField] public static Color tier3Color = new Color(0.8470588f, 0.2117647f, 0.5137255f);

    [Header("Component Vars")]
    public Image panel;
    public Slider slider;
    public Image icon;

    [Space(20)]
    [Header("Script Vars")]
    [SerializeField]private GameObject weaponPrefab;
    [SerializeField] private int tier;

    private Button button;
    private bool isEquiped;

    EquipableWeaponPanelScript equipableWeaponPanel;


    public void Start()
    {
        button = GetComponent<Button>();

        equipableWeaponPanel = GetComponentInParent<EquipableWeaponPanelScript>();


    }



    public void EquipWeapon()
    {
        isEquiped = true;
        button.interactable = !isEquiped;
        equipableWeaponPanel.EquipWeapon(this);
    }

    public void UnequipWeapon()
    {
        isEquiped = false;
        button.interactable = !isEquiped;
    }


    public GameObject GetWeapon()
    {
        return weaponPrefab;
    }

    public void InitalizeWidget(WeaponInformation weaponInformation)
    {
        tier = weaponInformation.tier;
        weaponPrefab = weaponInformation.weapon;
        icon.sprite = weaponInformation.icon;

        if (tier == 1)
        {
            print("running");
            panel.color = tier1Color;

        } else if(tier == 2)
        {
            panel.color = tier2Color;
        } else if(tier == 3)
        {
            panel.color = tier3Color;
        }

    }

    public int GetTier()
    {
        return tier;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionPanelScript : MonoBehaviour
{

    public Button[] optionButtons;
    public Image[] optionImages;

    public List<WeaponInformation> weaponOptions;

    public Sprite nullOption;

    private Player_InventoryScript inventoryScript;

    private void Awake()
    {
        optionImages = new Image[optionButtons.Length];
        for (int i = 0; i < optionImages.Length; i++)
        {
            optionImages[i] = optionButtons[i].GetComponent<Image>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryScript = GetComponentInParent<Player_InventoryScript>();

    }

    public void NewSelection(in List<WeaponInformation> weapons)
    {
        weaponOptions = weapons;
        RefreshSelection();
    }

    private void RefreshSelection()
    {
        for (int i = 0; i < optionImages.Length; i++)
        {
            optionImages[i].sprite = nullOption;
        }

        for (int i = 0; i < weaponOptions.Count; i++)
        {
            optionImages[i].sprite = weaponOptions[i].icon;
        }
    }

    public void SelectOption(int option)
    {
        inventoryScript.ChangeWeapon(weaponOptions[option]);
    }

}

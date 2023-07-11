using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EquipableWeaponPanelScript : MonoBehaviour
{

    [SerializeField] List<Slot> queue;

    public VerticalLayoutGroup layout;

    private EquipableWeaponsWidgetScript[] equipedWidgets = new EquipableWeaponsWidgetScript[3];

    public GameObject widgetPrefab;

    private EggShopScript eggShop;

    [SerializeField] Slot[] testWeapons;

    // Start is called before the first frame update
    void Start()
    {
        eggShop = GetComponentInParent<EggShopScript>();
    }


    public void EquipWeapon(EquipableWeaponsWidgetScript widget)
    {
        int index = widget.GetTier() - 1;
        if (equipedWidgets[index] != null)
        {
            equipedWidgets[index].UnequipWeapon();
        }

        equipedWidgets[index] = widget;
        ///eggShop.GetPlayer().weaponManager.GiveNewGun(widget.GetWeapon(), widget.GetTier());

    }


    /*
     * TODO POSSIBLY REDESIGN WHOLE SYSTEM
     * QUEUE DOESN'T PUT THINGS IN ORDER
     * MAKE BUYING SYSTEM
     * 
     * 
     */

    public void AddWeapon(WeaponInformation weaponInformation)
    {
        GameObject widget = Instantiate(widgetPrefab, layout.transform);
        EquipableWeaponsWidgetScript widgetScript = widget.GetComponent<EquipableWeaponsWidgetScript>();
        widgetScript.InitalizeWidget(weaponInformation);
        //widgetScript
        int index = queue.Count;
        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i].weight >= weaponInformation.tier)
            {
                index = i;
                break;
            }
        }
        Slot slot = new Slot();
        slot.elem = widget;
        slot.weight = weaponInformation.tier;
        queue.Insert(index, slot);
        RefreshContainer();

    }

    
    private void RefreshContainer()
    {
        foreach (Slot element in queue)
        {
            element.elem.transform.SetAsLastSibling();
        }

        
    }


    [System.Serializable]
    struct Slot
    {
        public GameObject elem;
        public int weight;
    }
}

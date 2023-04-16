using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TierWeaponWidget : MonoBehaviour
{

    public GridLayoutGroup layout;
    public TMP_Text costText;

    public int cost;

    public WeaponInformation[] weapons;

    private EggShopScript eggShop;

    private LongClickButton_Image longClick;

    public GameObject purchasedImage;

    private bool canPurchase = true;



    // Start is called before the first frame update
    void Start()
    {
        eggShop = GetComponentInParent<EggShopScript>();

        longClick = GetComponent<LongClickButton_Image>();

        costText.text = cost + "";

        foreach (var item in weapons)
        {
            GameObject imageObj = new GameObject();
            imageObj.transform.SetParent(layout.transform);
            Image weaponImage = imageObj.AddComponent<Image>();
            weaponImage.sprite = item.icon;
            imageObj.transform.localScale = Vector3.one;
        }
    }

    public void PurchaseWeaponTier()
    {
        PlayerScript player = eggShop.GetPlayer();
        foreach (var item in weapons)
        {
            player.weaponManager.GiveNewGun(item);
        }

        purchasedImage.SetActive(true);
        longClick.SetCanLongClick(false);
        canPurchase = false;
    }

    public void CanPurchaseTier()
    {
        if (!canPurchase)
        {
            return;
        }
        PlayerScript player = eggShop.GetPlayer();
        if (player.CanAffordSoulFire(cost))
        {
            print("can purchase");
            longClick.SetCanLongClick(true);
        } else
        {
            print("can't afford");
            longClick.SetCanLongClick(false);
            //spawn can't afford effect
            //floating text that fades
        }
    }

}

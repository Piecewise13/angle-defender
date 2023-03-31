using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseableWeaponWidget : MonoBehaviour
{

    [SerializeField] public static Color tier1Color = new Color(0.2941177f, 0.254902f, 0.7176471f);
    [SerializeField] public static Color tier2Color = new Color(0.5019608f, 0.2039216f, 0.6941177f);
    [SerializeField] public static Color tier3Color = new Color(0.8470588f, 0.2117647f, 0.5137255f);

    public TMP_Text cost;
    public TMP_Text weaponName;

    public Image icon;
    public Image holdIndicator;

    public Image panel;

    [SerializeField] private WeaponInformation information;

    private PurchaseableWeaponPanelScript panelScript;

    


    // Start is called before the first frame update
    void Start()
    {
        panelScript = GetComponentInParent<PurchaseableWeaponPanelScript>();
        holdIndicator.fillAmount = 0f;
    }

    public void InitializeWidget(WeaponInformation info)
    {
        cost.text = info.cost + "";
        weaponName.text = info.name;
        icon.sprite = info.icon;
        information = info;


        if (info.tier == 1)
        {
            print("running");
            panel.color = tier1Color;

        }
        else if (info.tier == 2)
        {
            panel.color = tier2Color;
        }
        else if (info.tier == 3)
        {
            panel.color = tier3Color;
        }

    }

    public void PurchaseWeapon()
    {
        if (panelScript.PurchaseWeapon(information))
        {
            Destroy(gameObject);
        }
    }

    public void ShowWeaponInformation()
    {

    }

}

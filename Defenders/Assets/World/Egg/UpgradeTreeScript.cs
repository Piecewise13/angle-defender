using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class UpgradeTreeScript : MonoBehaviour
{

    string[][] perkText;

    private ParentPerkScript[] perks;

    private ParentPerkScript focusedPerk;
    private bool canAffordPerk;


    private PlayerScript player;
    public EggScript egg;

   

    [Header("PerkInfo")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    //[Header("CostInfo")]
    //public TMP_Text woodCost;
    //public TMP_Text ironCost;
    //public TMP_Text diamondCost;

    [Header("Player Info")]
    public TMP_Text currentFireText;
    public TMP_Text purchaseText;
    public Button purchaseButton;



    int selectedPerk;

    // Start is called before the first frame update
    void Start()
    {
        //perks = FindObjectsOfType<ParentPerkScript>();
        perks = GetComponentsInChildren<ParentPerkScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void upgradeButtonPressed(ParentPerkScript script)
    {
        focusedPerk = script;
        nameText.text = focusedPerk.perkName;
        descriptionText.text = focusedPerk.perkDescription;

        if (focusedPerk.isUnlocked)
        {
            return;

        }
        if (focusedPerk.isAvailale)
        {
            canAffordPerk = CanAfford(script);
        }


        

        //woodCost.text = script.woodCost.ToString();
        //ironCost.text = script.ironCost.ToString();
        //diamondCost.text = script.diamondCost.ToString();



    }

    public void purchaseUpgrade()
    {
        print(focusedPerk);
        if (!focusedPerk.isUnlocked)
        {
            if (CanAfford(focusedPerk))
            {

                //player.SetResourceAmount(ResourceType.Wood, -focusedPerk.woodCost);
                //player.SetResourceAmount(ResourceType.Iron, -focusedPerk.ironCost);
                //player.SetResourceAmount(ResourceType.Diamond, -focusedPerk.diamondCost);
                player.SetSoulFire(-focusedPerk.soulFireCost);
                currentFireText.text = "" + player.GetSoulFire();
                focusedPerk.UnlockUpgrade(player);

                if (focusedPerk.id == -1)
                {
                    if (focusedPerk.isUnlocked)
                    {
                        return;

                    }
                } else
                {
                    focusedPerk.Unlocked();
                }
                
                OpenPath(focusedPerk);
            }
            
        }
    }

    private bool CanAfford(ParentPerkScript perk)
    {
        //if(player.GetResourceAmount(ResourceType.Wood) >= perk.woodCost &&
        //    player.GetResourceAmount(ResourceType.Iron) >= perk.ironCost &&
        //    player.GetResourceAmount(ResourceType.Diamond) >= perk.diamondCost)
        //{
        //    return true;
        //}
        //return false;
        if (perk.soulFireCost > player.GetSoulFire())
        {
            perk.longClick.canLongClick = false;
            StartCoroutine((perk.CostTextFlash()));
            return false;
        }
        perk.longClick.canLongClick = true;

        return true;

    }

    private void OpenPath(ParentPerkScript perk)
    {

        int perkOffset = 0;

        if (perk.id < 5 && perk.id >= 0)
        {
            perkOffset = 3;
            perks[perk.id + perkOffset].SetAvalible();

        }
        else if (perk.id >= 6 && perk.id < 9)
        {
            perkOffset = 3 + (perk.id % 6);
            perks[perk.id + perkOffset].SetAvalible();
            perks[perk.id + perkOffset + 1].SetAvalible();

        }
        else if (perk.id >= 9 && perk.id < 15)
        {
            if (perk.id % 2 != 0)
            {
                perkOffset = 6;
                perks[perk.id + 1].SetUnavalible();
                perks[perk.id + perkOffset].SetAvalible();

            } else
            {
                perkOffset = 6;
                perks[perk.id - 1].SetUnavalible();
                perks[perk.id + perkOffset].SetAvalible();

            }

        } else if (perk.id >=15)
        {
            perkOffset = 5;
            perks[perk.id + perkOffset].SetAvalible();
        }


        //int perkOffset = 0;

        //if (perk.id < 10)
        //{
        //    perkOffset = 3;

        //}
        //else if(perk.id >= 10 && perk.id < 16)
        //{
        //    perkOffset = 3 + (perk.id % 10);
        //} else if(perk.id >= 16)
        //{
        //    perkOffset = 6;
        //}
        //for (int i = 0; i < perks.Length; i++)
        //{
        //    if (perks[i].id == perk.id - perkOffset)
        //    {
        //        return perks[i].isUnlocked;
        //    }
        //}
        //return true;
    }


    private void OnEnable()
    {
        player = egg.player;
        currentFireText.text = player.GetSoulFire() + "";
    }


}

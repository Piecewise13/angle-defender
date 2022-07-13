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



    private PlayerScript player;
    public EggScript egg;

    [Header("PerkInfo")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    [Header("CostInfo")]
    public TMP_Text woodCost;
    public TMP_Text ironCost;
    public TMP_Text diamondCost;



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

        woodCost.text = script.woodCost.ToString();
        ironCost.text = script.ironCost.ToString();
        diamondCost.text = script.diamondCost.ToString();

    }

    public void purchaseUpgrade()
    {
        if (!focusedPerk.isUnlocked)
        {
            if (CanAfford(focusedPerk))
            {
                
                player.SetResourceAmount(ResourceType.Wood, -focusedPerk.woodCost);
                player.SetResourceAmount(ResourceType.Iron, -focusedPerk.ironCost);
                player.SetResourceAmount(ResourceType.Diamond, -focusedPerk.diamondCost);
                focusedPerk.UnlockUpgrade(player);
                OpenPath(focusedPerk);
            }
            else
            {
                print("not enough resources");
            }
            
        }
        else
        {
            print("already unlocked");
        }
    }

    private bool CanAfford(ParentPerkScript perk)
    {
        if(player.GetResourceAmount(ResourceType.Wood) >= perk.woodCost &&
            player.GetResourceAmount(ResourceType.Iron) >= perk.ironCost &&
            player.GetResourceAmount(ResourceType.Diamond) >= perk.diamondCost)
        {
            return true;
        }
        return false;

    }

    private void OpenPath(ParentPerkScript perk)
    {

        int perkOffset = 0;

        if (perk.id < 5)
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
    }


}

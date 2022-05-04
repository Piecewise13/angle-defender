using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class UpgradeTreeScript : MonoBehaviour
{

    string[][] perkText;

    //public ParentPerkScript[] perks;

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
        if (!focusedPerk.unlocked)
        {
            if (hasEnough(focusedPerk))
            {
                
                player.updateResourceAmount(ResourceType.Wood, -focusedPerk.woodCost);
                player.updateResourceAmount(ResourceType.Iron, -focusedPerk.ironCost);
                player.updateResourceAmount(ResourceType.Diamond, -focusedPerk.diamondCost);
                focusedPerk.UnlockUpgrade();
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

    private bool hasEnough(ParentPerkScript perk)
    {
        if(player.getResourceAmount(ResourceType.Wood) > perk.woodCost ||
            player.getResourceAmount(ResourceType.Iron) > perk.ironCost ||
            player.getResourceAmount(ResourceType.Diamond) > perk.diamondCost)
        {
            return true;
        }
        return false;

    }

    private void OnEnable()
    {
        player = egg.player;
    }


}

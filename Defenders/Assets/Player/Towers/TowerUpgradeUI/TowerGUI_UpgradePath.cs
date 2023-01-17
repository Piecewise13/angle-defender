using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TowerGUI_UpgradePath : MonoBehaviour
{ 
    [Header("Selection Box")]
    public Image selectionImage;
    public Sprite SelectedBox;
    public Sprite UnselectedBox;

    [Header("Indicator Box")]
    public static float[] FILL_AMOUNTS = {0, .085f, .13f, .2f, .3f, .37f, .42f, .5f, .58f, .63f, .69f, .8f, .87f, .92f, 1f };
    public Image indicatorImage;
    public Sprite maxedOutSprite;

    [Header("Upgrade Information")]
    [SerializeField] private int[] costs;
    protected int upgradeCount;

    
    /*
     * OLD SYSTEM
     */
    public UpgradeInfo[] upgrades;
    public int currentUpgradeAvalible;

    private bool[] unlocked;




    [Header("UI Vars")]
    public Image upgradeIcon;

    public TMP_Text upgradeCost;

    private TowerGUI_Interactable interactable;
    private TowerGUIParent GUIParent;



    // Start is called before the first frame update
    public virtual void Start()
    {
        interactable = GetComponent<TowerGUI_Interactable>();
        
        GUIParent = GetComponentInParent<TowerGUIParent>();
        //unlocked = new bool[upgrades.Length];
        ChangeUpgradeInfo();
        UnselectPath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void SpecialFunctionality();


    public void UpgradeBought()
    {
        if (upgradeCount >= 10)
        {
            return;
        }

        upgradeCount++;
        ChangeUpgradeInfo();
        SpecialFunctionality();

        //set it uninteractable if it is the last upgrade and set new image boarder

        /*
        if (upgrade == 0)
        {
            UpgradeOne();
        } else if (upgrade == 1)
        {
            UpgradeTwo();
        } else if (upgrade == 2)
        {
            UpgradeThree();
        } else
        {
            print("over");
            ChangeUpgradeInfo(upgrade++);
            return;
        }
        upgradeTickBoxes[upgrade].color = upgradeTickBoxColor;
        currentUpgradeAvalible++;
        ChangeUpgradeInfo(currentUpgradeAvalible);
        interactable.Interact();
        
        */

    }

    protected void ChangeUpgradeInfo()
    {

        //figure out way to determine the next price. either by formula or preset values
        indicatorImage.fillAmount = FILL_AMOUNTS[upgradeCount];
        upgradeCost.text = costs[upgradeCount] + "";

        /*
        if (nextUpgrade != upgrades.Length)
        {

            upgradeCost.text = upgrades[nextUpgrade].cost + "";
            upgradeIcon = upgrades[nextUpgrade].icon;
        } else
        {
            interactable.enabled = false;
            //set it equal to finsihed graphic and info 
        }
        */
    }

    public void SelectPath()
    {
        selectionImage.sprite = SelectedBox;
    }

    public void UnselectPath()
    {
        selectionImage.sprite = UnselectedBox;
    }

    public int GetCurrentCost()
    {
        return costs[upgradeCount];
    }

    public int GetCurrentUpgradeNumber()
    {
        return upgradeCount;
    }

}


[System.Serializable]
public class UpgradeInfo
{
    [SerializeField] public string name;
    [TextArea]
    [SerializeField] public string description;
    [SerializeField] public int cost;
    [SerializeField] public Image icon;

}
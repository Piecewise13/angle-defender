using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TowerGUI_UpgradePath : MonoBehaviour
{ 
    [Header("Selection Box")]
    public Image selectionImage;
    private Sprite SelectedBox;
    private Sprite UnselectedBox;

    [Header("Indicator Box")]
    public static float[] FILL_AMOUNTS = {0, 0.123f, 0.186f, 0.309f, 0.378f, 0.516f, 0.624f, 0.709f, 0.817f, 0.902f, 1f};
    public Image indicatorImage;
    public Sprite maxedOutSprite;

    [Header("Upgrade Information")]
    [SerializeField] private int[] costs;
    protected int upgradeCount;
    public string descriptionText;

   




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

        SelectedBox = Resources.Load<Sprite>("TowerGUI/TowerUI_SelectedRect");
        UnselectedBox = Resources.Load<Sprite>("TowerGUI/TowerUI_UnselectedRect");
        selectionImage.sprite = UnselectedBox;
    }


    public abstract void SpecialFunctionality();

    public bool CanBuyUpgrade()
    {
        return upgradeCount <= 10;
    }


    public void Upgrade()
    {
        upgradeCount++;



        ChangeUpgradeInfo();
        SpecialFunctionality();

    }

    protected void ChangeUpgradeInfo()
    {

        //figure out way to determine the next price. either by formula or preset values
        indicatorImage.fillAmount = FILL_AMOUNTS[upgradeCount];
        if (upgradeCount > costs.Length)
        {
            upgradeCost.text = "MAXED";
        }
        upgradeCost.text = costs[upgradeCount] + "";
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
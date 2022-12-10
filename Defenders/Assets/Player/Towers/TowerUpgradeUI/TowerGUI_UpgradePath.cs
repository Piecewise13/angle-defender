using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TowerGUI_UpgradePath : MonoBehaviour
{

    public Image[] upgradeTickBoxes;
    public static Color upgradeTickBoxColor = new Color(0f, 255f, 0f, 255f);

    public Image selectionImage;
    public static Color selectedColor = new Color(140f, 85f, 135f, .1f);
    public static Color unselectedColor = new Color(140f, 85f, 135f, 0f);

    public UpgradeInfo[] upgrades;
    public int currentUpgradeAvalible;

    private bool[] unlocked;

    [Header("UI Vars")]
    public Image upgradeIcon;

    public TMP_Text upgradeCost;

    private TowerGUI_Interactable interactable;
    private TowerGUIParent GUIParent;


    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<TowerGUI_Interactable>();
        
        GUIParent = GetComponentInParent<TowerGUIParent>();
        //unlocked = new bool[upgrades.Length];
        ChangeUpgradeInfo(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void UpgradeOne();

    public abstract void UpgradeTwo();

    public abstract void UpgradeThree();


    public void UpgradeBought(int upgrade)
    {
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
        
        

    }

    protected void ChangeUpgradeInfo(int nextUpgrade)
    {
        if (nextUpgrade != upgrades.Length)
        {

            upgradeCost.text = upgrades[nextUpgrade].cost + "";
            upgradeIcon = upgrades[nextUpgrade].icon;
        } else
        {
            interactable.enabled = false;
            //set it equal to finsihed graphic and info 
        }

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
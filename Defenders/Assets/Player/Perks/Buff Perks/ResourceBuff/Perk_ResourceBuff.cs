using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_ResourceBuff : ParentPerkScript
{

    public EggScript egg;
    private Perk_BuffIndicator indicator;
    private int upgradeInt;

    new void Start()
    {
        base.Start();
        indicator = GetComponentInChildren<Perk_BuffIndicator>();
        SetAvalible();
    }

    public override void UnlockUpgrade(PlayerScript player)
    {
        egg.UpgradeResourceRate(upgradeInt);
        indicator.IncreaseIndicator();
        upgradeInt++;
        soulFireCost += (int)(soulFireCost * (Mathf.Pow(1.0025f, upgradeInt) - .75f));
        //woodCost = (int)(woodCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        //ironCost = (int)(ironCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        //diamondCost = (int)(diamondCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        costText.text = soulFireCost + "";
        if (upgradeInt == 10)
        {
            //longClick.SetCanLongClick();
            isUnlocked = true;
        }
    }
}

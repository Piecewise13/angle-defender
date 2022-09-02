using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_ResourceBuff : ParentPerkScript
{

    public EggScript egg;

    private int upgradeInt;

    new void Start()
    {
        base.Start();
        SetAvalible();
    }

    public override void UnlockUpgrade(PlayerScript player)
    {
        egg.UpgradeResourceRate();
        upgradeInt++;
        woodCost = (int)(woodCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        ironCost = (int)(ironCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        diamondCost = (int)(diamondCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        if(upgradeInt == 10)
        {
            isUnlocked = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

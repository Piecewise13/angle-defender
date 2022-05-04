using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_ResourceBuff : ParentPerkScript
{

    public EggScript egg;

    private int upgradeInt;

    public override void UnlockUpgrade()
    {
        egg.UpgradeResourceRate();
        upgradeInt++;
        woodCost = (int)(woodCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        ironCost = (int)(ironCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        diamondCost = (int)(diamondCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        if(upgradeInt == 10)
        {
            unlocked = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

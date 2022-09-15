using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_PassiveSoulFire : ParentPerkScript
{

    private int upgradeInt;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        SetAvalible();
    }

    public override void UnlockUpgrade(PlayerScript player)
    {
        upgradeInt++;
        // g(x) = 25x + 15 for a total soulfire value of 2025
        player.SetSoulFireMax(30 * (upgradeInt) + 20);
        soulFireCost += (int)(soulFireCost * (Mathf.Pow(1.0025f, upgradeInt) - .75f));

        //woodCost = (int)(woodCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        //ironCost = (int)(ironCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        //diamondCost = (int)(diamondCost * (Mathf.Pow(upgradeInt, 1.5f) + 1));
        if (upgradeInt == 10)
        {
            isUnlocked = true;
        }
    }
}

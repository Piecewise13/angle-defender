using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_PassiveHealthBuff : ParentPerkScript
{
    private int upgradeInt;
    Perk_BuffIndicator indicator;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        indicator = GetComponentInChildren<Perk_BuffIndicator>();
        SetAvalible();
    }

    public override void UnlockUpgrade(PlayerScript player)
    {
        upgradeInt++;
        // g(x) = 20x for a total soulfire value of 1200
        player.SetHealthMax(20 * (upgradeInt));

        //soul fire cost from starting 100 with exponential curve

        soulFireCost += (int)(soulFireCost * (Mathf.Pow(1.0025f, upgradeInt) - .75f));
        indicator.IncreaseIndicator();
    
        if (upgradeInt == 10)
        {
            isUnlocked = true;
        }
    }
}

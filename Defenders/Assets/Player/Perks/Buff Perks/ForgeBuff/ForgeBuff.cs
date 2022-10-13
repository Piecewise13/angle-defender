using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeBuff : ParentPerkScript
{

    private BulletForge forge;
    private int upgradeInt;

    Perk_BuffIndicator indicator;

    public override void UnlockUpgrade(PlayerScript player)
    {

        soulFireCost += (int)(soulFireCost * (Mathf.Pow(1.0025f, upgradeInt) - .75f));
        upgradeInt++;
        forge.UpgradeForge(upgradeInt);
        indicator.IncreaseIndicator();
        costText.text = soulFireCost + "";
        if (upgradeInt == 10)
        {
            Unlocked();
        }


    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        forge = FindObjectOfType<BulletForge>();
        indicator = GetComponentInChildren<Perk_BuffIndicator>();
        SetAvalible();
    }
}

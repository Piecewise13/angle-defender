using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeBuff : ParentPerkScript
{

    private BulletForge forge;
    private int upgradeInt;

    public override void UnlockUpgrade(PlayerScript player)
    {

        soulFireCost += (int)(soulFireCost * (Mathf.Pow(1.0025f, upgradeInt) - .75f));
        upgradeInt++;
        forge.UpgradeForge(upgradeInt);
        if (upgradeInt == 10)
        {
            isUnlocked = true;
        }


    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        forge = FindObjectOfType<BulletForge>();

        SetAvalible();
    }
}

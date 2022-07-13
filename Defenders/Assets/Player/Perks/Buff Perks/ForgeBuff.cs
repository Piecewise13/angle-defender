using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeBuff : ParentPerkScript
{

    private BulletForge forge;
    private int upgradeNum;

    public override void UnlockUpgrade(PlayerScript player)
    {


        upgradeNum++;
        forge.UpgradeForge(upgradeNum);
        if (upgradeNum == 10)
        {
            isUnlocked = true;
        }


    }

    // Start is called before the first frame update
    new void Start()
    {
        forge = FindObjectOfType<BulletForge>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

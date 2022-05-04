using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeBuff : ParentPerkScript
{

    private BulletForge forge;
    private int upgradeNum;

    public override void UnlockUpgrade()
    {


        upgradeNum++;
        forge.UpgradeForge(upgradeNum);
        if (upgradeNum == 10)
        {
            unlocked = true;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        forge = FindObjectOfType<BulletForge>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

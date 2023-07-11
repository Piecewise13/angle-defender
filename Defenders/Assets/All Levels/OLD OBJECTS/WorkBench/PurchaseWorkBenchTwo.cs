using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseWorkBenchTwo : PurchaseableParent
{
    public WorkBenchScript wbScript;

    public override void Purchase()
    {
        wbScript.TierUnlocked(2);
        base.Purchase();
    }

}

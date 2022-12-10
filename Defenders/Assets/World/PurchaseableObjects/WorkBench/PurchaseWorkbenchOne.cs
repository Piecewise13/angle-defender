using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseWorkbenchOne : PurchaseableParent
{

    public WorkBenchScript wbScript;

    public GameObject upgradeTwoObj;
    

    public override void Purchase()
    {
        upgradeTwoObj.SetActive(true);

        base.Purchase();
    }
}

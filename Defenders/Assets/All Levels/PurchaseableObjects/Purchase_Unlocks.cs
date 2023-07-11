using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase_Unlocks : PurchaseableParent
{
    public GameObject unlocksObject;

    public override void Purchase()
    {
        unlocksObject.SetActive(true);
        base.Purchase();
    }
}

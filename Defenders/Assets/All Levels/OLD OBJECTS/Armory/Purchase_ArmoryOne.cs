using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase_ArmoryOne : PurchaseableParent
{
    public GameObject armoryTwo;

    public override void Purchase()
    {

        armoryTwo.SetActive(true);
        base.Purchase();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseWeaponDamage : ParentPerkScript
{

    private WeaponInventoryManager manager;

    public override void UnlockUpgrade(PlayerScript player)
    {
        //BasicWeaponScript.damageMultiplier = 1.1f;
    }



    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        manager = GetComponentInParent<WeaponInventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

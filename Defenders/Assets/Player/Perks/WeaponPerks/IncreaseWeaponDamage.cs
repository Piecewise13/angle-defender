using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseWeaponDamage : ParentPerkScript
{

    private WeaponInventoryManager manager;

    public override void UnlockUpgrade()
    {
        BasicWeaponScript.damageMultiplier = 1.5f;
    }



    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<WeaponInventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

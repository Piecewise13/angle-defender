using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUpgrades : ParentPerkScript
{
    public override void UnlockUpgrade(PlayerScript player)
    {
        player.UnlockFirstUpgrade();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

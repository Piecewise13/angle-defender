using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_ReinforcedWalls2 : ParentPerkScript
{
    public override void UnlockUpgrade(PlayerScript player)
    {
        ParentAIScript.UpdateWalls();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

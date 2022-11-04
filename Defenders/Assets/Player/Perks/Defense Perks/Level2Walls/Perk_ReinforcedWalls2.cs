using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_ReinforcedWalls2 : ParentPerkScript
{
    public override void UnlockUpgrade(PlayerScript player)
    {
        WallDefenceScript.cost.Clear();
        WallDefenceScript.cost.Add(ResourceType.Wood, 40);
        WallDefenceScript.cost.Add(ResourceType.Iron, 20);
        WallDefenceScript.cost.Add(ResourceType.Diamond, 0);
        ParentAIScript.UpdateWalls();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

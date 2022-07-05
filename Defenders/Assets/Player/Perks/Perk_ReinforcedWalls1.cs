using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_ReinforcedWalls1 : ParentPerkScript
{
    public override void UnlockUpgrade()
    {
        WallDefenceScript.cost.Clear();
        WallDefenceScript.cost.Add(ResourceType.Wood, 20);
        WallDefenceScript.cost.Add(ResourceType.Iron, 0);
        WallDefenceScript.cost.Add(ResourceType.Diamond, 0);
        ParentAIScript.UpdateWalls();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

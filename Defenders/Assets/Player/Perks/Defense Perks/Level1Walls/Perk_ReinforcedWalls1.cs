using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_ReinforcedWalls1 : ParentPerkScript
{
    public GameObject defenses;

    public override void UnlockUpgrade(PlayerScript player)
    {
        defenses.SetActive(true);
        WallDefenceScript.cost.Clear();
        WallDefenceScript.cost.Add(ResourceType.Wood, 20);
        WallDefenceScript.cost.Add(ResourceType.Iron, 0);
        WallDefenceScript.cost.Add(ResourceType.Diamond, 0);
        ParentAIScript.UpdateWalls();
    }

    private new void Start()
    {
        base.Start();
        SetAvalible();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tier1Towers : ParentPerkScript
{
    private WorkBenchScript script;

    public override void UnlockUpgrade(PlayerScript player)
    {
        script = FindObjectOfType<WorkBenchScript>();
        script.TierUnlocked(1);
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        SetAvalible();

    }
}

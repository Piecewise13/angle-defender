using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPerk : ParentPerkScript
{
    public override void UnlockUpgrade(PlayerScript player)
    {
        print("unlocking test");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

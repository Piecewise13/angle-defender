using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngotPerkUpgrade : ParentPerkScript
{
    public override void UnlockUpgrade(PlayerScript player)
    {
        IngotScript.PlayerUpgrade();
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}

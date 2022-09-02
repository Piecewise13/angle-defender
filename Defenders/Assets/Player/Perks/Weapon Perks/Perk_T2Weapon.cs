using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_T2Weapon : ParentPerkScript
{

    public GameObject armory;

    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UnlockUpgrade(PlayerScript player)
    {
        armory.SetActive(true);
        
    }
}

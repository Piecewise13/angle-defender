using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_TurretLevel1 : ParentPerkScript
{

    public TurretScript[] turrets;


    public override void UnlockUpgrade(PlayerScript player)
    {

        foreach (TurretScript turret in turrets)
        {
            turret.gameObject.SetActive(true);
        }
        isUnlocked = true;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

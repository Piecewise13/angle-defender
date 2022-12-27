using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_OneUpgradeScript : TurretUpgradeScript
{
    public override void Upgrade()
    {
        print("upgrade");
    }

    public void ShootMiddleMan()
    {
        mainScript.Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

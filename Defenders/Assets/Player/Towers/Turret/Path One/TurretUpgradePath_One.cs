using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradePath_One : TowerGUI_UpgradePath
{
    public GameObject[] turrets;

    public GameObject currentTurret;

    public override void UpgradeOne()
    {
        print("upgrade one");
        Destroy(currentTurret);
        currentTurret = Instantiate(turrets[0], transform.root);
    }



    public override void UpgradeTwo()
    {
        print("upgrade two");
        Destroy(currentTurret);
        currentTurret = Instantiate(turrets[1], transform.root);
    }

    public override void UpgradeThree()
    {
        print("upgrade three");
        Destroy(currentTurret);
        currentTurret = Instantiate(turrets[2], transform.root);
    }

}

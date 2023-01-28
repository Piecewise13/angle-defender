using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradePath_Speed : TowerGUI_UpgradePath
{
    public GameObject[] models;

    private int modelsIndex;

    public GameObject currentTurret;

    private Animator anim;

    private TurretScript turret;

    int modelCount = 0;

    public override void Start()
    {
        base.Start();
        turret = GetComponentInParent<TurretScript>();
    }


    public override void SpecialFunctionality()
    {
        print(modelCount);
        if (modelCount == 3)
        {
            modelCount = 0;
            //change model and animation for model
            
            turret.ChangeBody(models[modelsIndex]);
            modelsIndex++;
        }
        modelCount++;

        turret.SetSpeedMultiplier(SpeedFormula());

        //turret.SetAnimator(anim);

    }

    private float SpeedFormula()
    {
        return .1f * upgradeCount + 1f;
    }
}

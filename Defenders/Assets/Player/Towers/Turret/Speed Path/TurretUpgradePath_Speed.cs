using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradePath_Speed : TowerGUI_UpgradePath
{
    public GameObject[] models;

    private int modelsIndex;

    private Animator anim;

    private TurretScript turret;

    public float[] speedValues;

    int modelCount = 0;

    public override void Start()
    {
        base.Start();
        turret = GetComponentInParent<TurretScript>();
    }


    public override void SpecialFunctionality()
    {
        //change model and animation for model

        turret.ChangeBody(models[modelsIndex]);
        modelsIndex++;

        turret.SetSpeedMultiplier(speedValues[upgradeCount - 1]);

        //turret.SetAnimator(anim);

    }

    private float SpeedFormula()
    {
        return .1f * upgradeCount + 1f;
    }
}

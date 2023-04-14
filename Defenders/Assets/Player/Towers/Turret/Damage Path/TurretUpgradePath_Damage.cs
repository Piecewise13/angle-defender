using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradePath_Damage : TowerGUI_UpgradePath
{
    private TurretScript turret;

    [Header("Damage Vars")]
    [SerializeField] private float[] damageValues;

    public GameObject[] models;
    private int modelsIndex;

    public override void Start()
    {
        base.Start();
        turret = GetComponentInParent<TurretScript>();
    }

    public override void SpecialFunctionality()
    {
        turret.ChangeBase(models[modelsIndex]);
        modelsIndex++;
        turret.SetDamageMultiplier(damageValues[upgradeCount - 1]);
        //print("Setting multiplier to " + damageValues[upgradeCount]);
    }
}

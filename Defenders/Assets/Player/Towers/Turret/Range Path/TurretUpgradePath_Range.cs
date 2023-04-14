using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradePath_Range : TowerGUI_UpgradePath
{
    private TurretScript turret;

    [Header("Range Vars")]
    [SerializeField] private float[] rangeValues;

    public GameObject[] models;
    private int modelsIndex;

    public override void SpecialFunctionality()
    {
        //print("md" + modelsIndex);
        turret.ChangeBarrels(models[modelsIndex]);
        modelsIndex++;
        turret.SetSeachRadius(rangeValues[upgradeCount - 1]);
        //print("Setting range multiplier to " + rangeValues[upgradeCount]);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        turret = GetComponentInParent<TurretScript>();
    }
}

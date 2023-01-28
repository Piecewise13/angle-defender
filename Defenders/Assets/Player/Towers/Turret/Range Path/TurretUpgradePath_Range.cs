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
        if (upgradeCount % models.Length == 0)
        {
            //change model and animation for model
            
            turret.ChangeBarrels(models[modelsIndex]);
            modelsIndex++;
        }
        turret.SetSeachRadius(rangeValues[upgradeCount]);
        //print("Setting range multiplier to " + rangeValues[upgradeCount]);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        turret = GetComponentInParent<TurretScript>();
    }
}

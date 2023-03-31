using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealerUpgradePath_Range : TowerGUI_UpgradePath
{
    private WallHealer_Script wallHealer;

    [Header("Range Vars")]
    [SerializeField] private float[] rangeValues;

    public GameObject[] models;
    private int modelsIndex;

    public override void SpecialFunctionality()
    {
        if (upgradeCount % models.Length == 0)
        {
            //change model and animation for model

            wallHealer.ChangeAntenna(models[modelsIndex]);
            modelsIndex++;
        }
        wallHealer.UpgradeRange(rangeValues[upgradeCount]);
        //print("Setting range multiplier to " + rangeValues[upgradeCount]);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        wallHealer = GetComponentInParent<WallHealer_Script>();
    }
}

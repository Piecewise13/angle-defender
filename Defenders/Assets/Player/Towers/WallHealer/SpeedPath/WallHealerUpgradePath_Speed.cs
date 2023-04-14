using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealerUpgradePath_Speed : TowerGUI_UpgradePath
{
    private WallHealer_Script wallHealer;

    [Header("Speed Vars")]
    [SerializeField] private float[] speedValues;

    public GameObject[] models;
    private int modelsIndex;

    public override void SpecialFunctionality()
    {
        wallHealer.ChangeStorage(models[modelsIndex]);
        wallHealer.IncreaseMaxRobots();
        modelsIndex++;
        wallHealer.UpgradeSpeed(speedValues[upgradeCount]);
        //print("Setting range multiplier to " + rangeValues[upgradeCount]);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        wallHealer = GetComponentInParent<WallHealer_Script>();
    }
}

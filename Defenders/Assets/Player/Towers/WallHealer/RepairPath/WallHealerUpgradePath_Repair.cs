using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealerUpgradePath_Repair : TowerGUI_UpgradePath
{
    private WallHealer_Script wallHealer;

    [Header("Repair Vars")]
    [SerializeField] private float[] repairValues;

    public GameObject[] models;
    private int modelsIndex;

    public override void SpecialFunctionality()
    {
        wallHealer.ChangeRobot(models[0]);
        modelsIndex++;
        wallHealer.UpgradeRepairAmount(repairValues[upgradeCount - 1]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace_Output_UpgradePath : TowerGUI_UpgradePath
{
    private FurnaceTower furnance;

    [Header("Efficiency Vars")]
    [SerializeField] private float[] outputValues;

    public override void SpecialFunctionality()
    {
        furnance.UpgradeOutput(outputValues[upgradeCount], (float)upgradeCount/(float)outputValues.Length);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        furnance = GetComponentInParent<FurnaceTower>();
    }
}

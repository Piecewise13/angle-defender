using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace_Speed_UpgradePath : TowerGUI_UpgradePath
{
    private FurnaceTower furnance;

    [Header("Efficiency Vars")]
    [SerializeField] private float[] speedValues;

    public GameObject[] models;
    private int modelsIndex;

    public override void SpecialFunctionality()
    {
        furnance.ChangeDispenser(models[modelsIndex]);
        modelsIndex++;

        furnance.UpgradeSpeed(speedValues[upgradeCount]);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        furnance = GetComponentInParent<FurnaceTower>();
    }
}

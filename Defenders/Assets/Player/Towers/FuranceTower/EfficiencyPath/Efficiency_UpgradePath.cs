using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efficiency_UpgradePath : TowerGUI_UpgradePath
{

    private FurnaceTower furnance;

    [Header("Efficiency Vars")]
    [SerializeField] private float[] efficiencyValues;

    public GameObject[] models;
    private int modelsIndex;

    public override void SpecialFunctionality()
    {
        if (upgradeCount % models.Length == 0)
        {
            //change model and animation for model

            furnance.ChangeForge(models[modelsIndex]);
            modelsIndex++;
        }

        furnance.UpgradeEfficency(efficiencyValues[upgradeCount]);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        furnance = GetComponentInParent<FurnaceTower>();
    }
}

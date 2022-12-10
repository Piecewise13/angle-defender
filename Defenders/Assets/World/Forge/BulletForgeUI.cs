using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletForgeUI : MonoBehaviour
{

    public FurnaceTower forge;


    int playerWoodAmount;
    int playerIronAmount;
    int playerDiamondAmount;


    int depositWoodAmount;
    int depositIronAmount;
    int depositDiamondAmount;


    public Slider playerSoulFire;

    [Header("Meter Vars")]
    public Slider fuelSlider;
    public Slider fuelPotSlider;
    public Slider fireSlider;
    public Slider firePotSlider;
    public TMP_Text fuelIndicator;
    public TMP_Text fireIndicator;


    [Header("Player Text")]
    public TMP_Text playerWoodText;
    public TMP_Text playerIronText;
    public TMP_Text playerDiamondText;

    [Header("Deposit Text")]
    public TMP_Text depositWoodText;
    public TMP_Text depositIronText;
    public TMP_Text depositDiamondText;



    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        forge = GetComponentInParent<FurnaceTower>();
        UpdateFuelMeter();
        UpdateSoulFireMeter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void WoodButtonPress(int num) {
        if (num < 0 && depositWoodAmount <= 0)
        {
            return;
        }

        if (((depositWoodAmount + num) * forge.woodFuelAmount) + forge.fuelAmount > forge.fuelMax)
        {
            return;
        }
        if (playerWoodAmount - num < 0)
        {
            return;
        }

        depositWoodAmount += num;
        playerWoodAmount -= num;
        UpdateDepositValues();
        UpdatePlayerValues();
        UpdatePotentialFuelMeter();
    }
    public void IronButtonPress(int num)
    {
        if (num < 0 && depositIronAmount <= 0)
        {
            return;
        }

        if (((depositIronAmount + num) * forge.ironFuelAmount) + forge.fuelAmount > forge.fuelMax)
        {
            return;
        }
        if (playerIronAmount - num < 0)
        {
            return;
        }

        depositIronAmount += num;
        playerIronAmount -= num;
        UpdateDepositValues();
        UpdatePlayerValues();
        UpdatePotentialFuelMeter();
    }
    public void DiamondButtonPress(int num)
    {
        if (num < 0 && depositDiamondAmount <= 0)
        {
            return;
        }

        if (((depositDiamondAmount + num) * forge.diamondFuelAmount) + forge.fuelAmount > forge.fuelMax)
        {
            return;
        }
        if (playerDiamondAmount - num < 0)
        {
            return;
        }

        depositDiamondAmount += num;
        playerDiamondAmount -= num;
        UpdateDepositValues();
        UpdatePlayerValues();
        UpdatePotentialFuelMeter();
    }

    public void ResourceButtonPressed(int num)
    {
        ResourceType type = (ResourceType)1;
        switch (type)
        {
            case ResourceType.Wood:
                depositWoodAmount += num;
                playerWoodAmount -= num;

                break;
            case ResourceType.Iron:
                depositIronAmount += num;
                playerIronAmount -= num;
                break;
            case ResourceType.Diamond:
                depositDiamondAmount += num;
                playerDiamondAmount -= num;
                break;
        }

        UpdateDepositValues();
        UpdatePlayerValues();
    }

    public void DepositResources()
    {
        
        forge.DepositResources(ResourceType.Wood, depositWoodAmount);
        forge.DepositResources(ResourceType.Iron, depositIronAmount);
        forge.DepositResources(ResourceType.Diamond, depositDiamondAmount);
        ResetValues();
        UpdateDepositValues();
        UpdatePlayerValues();
        UpdatePotentialFuelMeter();
    }

    public void UpdateFuelMeter()
    {
        fuelSlider.value = (float)forge.fuelAmount / (float)forge.fuelMax;
        fuelIndicator.text = forge.fuelAmount + "";
        UpdatePotentialFuelMeter();
    }

    void UpdatePotentialFuelMeter()
    {
        float value = (depositWoodAmount * forge.woodFuelAmount) + (depositIronAmount * forge.ironFuelAmount) + (depositDiamondAmount * forge.diamondFuelAmount) + forge.fuelAmount;
        fuelPotSlider.value = value / forge.fuelMax;
    }

    public void UpdateSoulFireMeter()
    {
        fireSlider.value = (float)forge.fireStored / (float)forge.soulFireMax;
        fireIndicator.text = forge.fireStored + "";


    }


    void UpdatePlayerValues()
    {
        playerWoodText.text = playerWoodAmount + "";
        playerIronText.text = playerIronAmount + "";
        playerDiamondText.text = playerDiamondAmount + "";
    }

    void UpdateDepositValues()
    {
        depositWoodText.text = depositWoodAmount + "";
        depositIronText.text = depositIronAmount + "";
        depositDiamondText.text = depositDiamondAmount + "";
    }

    void ResetValues()
    {
        depositWoodAmount = 0;
        depositIronAmount = 0;
        depositDiamondAmount = 0;
        UpdatePotentialFuelMeter();
    }

    public void OnEnable()
    {
        PlayerScript player = forge.GetPlayer();
        if (player == null)
        {
            return;
        }
        playerWoodAmount = player.GetResourceAmount(ResourceType.Wood);
        playerIronAmount = player.GetResourceAmount(ResourceType.Iron);
        playerDiamondAmount = player.GetResourceAmount(ResourceType.Diamond);
        ResetValues();
        UpdateFuelMeter();
        UpdateSoulFireMeter();
        UpdatePlayerValues();
        UpdateDepositValues();

    }
}

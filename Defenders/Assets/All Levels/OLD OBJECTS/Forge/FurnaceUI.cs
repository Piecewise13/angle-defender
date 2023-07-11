using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FurnaceUI : MonoBehaviour
{

    public FurnaceTower forge;


    int playerDiamondAmount;

    int depositDiamondAmount;


    [Header("Meter Vars")]
    public Slider fuelSlider;
    public Slider fuelPotSlider;
    public TMP_Text fuelIndicator;


    [Header("Player Text")]
    public TMP_Text playerDiamondText;

    [Header("Deposit Text")]
    public TMP_Text depositDiamondText;


    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);

        UpdateFuelMeter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DiamondButtonPress(int num)
    {
        if (num < 0 && depositDiamondAmount <= 0)
        {
            return;
        }

        if (((depositDiamondAmount + num) * forge.GetResourceFuelAmount(ResourceType.Diamond)) + forge.GetFuelAmount() > forge.fuelMax)
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
        depositDiamondAmount += num;
        playerDiamondAmount -= num;

        UpdateDepositValues();
        UpdatePlayerValues();
    }

    public void DepositResources()
    {
        
        forge.DepositResources(ResourceType.Diamond, depositDiamondAmount);
        ResetValues();
        UpdateDepositValues();
        UpdatePlayerValues();
        UpdatePotentialFuelMeter();
        UpdateFuelMeter();
    }

    public void UpdateFuelMeter()
    {
        fuelSlider.value = (float)forge.GetFuelAmount() / (float)forge.fuelMax;
        fuelIndicator.text = forge.GetFuelAmount() + "";
        UpdatePotentialFuelMeter();
    }

    void UpdatePotentialFuelMeter()
    {
        float value = (depositDiamondAmount * forge.GetResourceFuelAmount(ResourceType.Diamond)) + forge.GetFuelAmount();
        fuelPotSlider.value = value / forge.fuelMax;
    }


    void UpdatePlayerValues()
    {
        playerDiamondText.text = playerDiamondAmount + "";
    }

    void UpdateDepositValues()
    {
        depositDiamondText.text = depositDiamondAmount + "";
    }

    void ResetValues()
    {
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
        playerDiamondAmount = player.GetDiamondAmount();
        ResetValues();
        UpdateFuelMeter();
        UpdatePlayerValues();
        UpdateDepositValues();

    }
}

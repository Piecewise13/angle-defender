using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletForgeUI : MonoBehaviour
{

    public BulletForge forge;
    
    private int ironAmount;
    private int depositAmount;


    public Slider slider;
    private float buttonPercent;


    public TMP_Text currentIron;
    public TMP_Text sliderMaxValue;
    public TMP_Text depositText;

    public TMP_Text playerIronIndicator;
    public TMP_Text turretIronIndicator;

    public GameObject restrictAccess;
    public Button turretButton;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sliderChange()
    {
        UpdateDepositAmount(Mathf.CeilToInt(ironAmount * slider.value));
    }

    public void percentButton(float percent)
    {
        buttonPercent = percent;
        UpdateDepositAmount(Mathf.CeilToInt(ironAmount * percent));
    }

    private void UpdateDepositAmount(int amount)
    {
        depositText.text = "Deposit: " + amount.ToString();
        depositAmount = amount;

    }

    public void DepositPlayer()
    {
        forge.ChangePlayerIronAmount(depositAmount);
        UpdateDepositAmount(0);
        UpdateIronIndicator();
    }

    public void DepositTurret()
    {
        forge.ChangeTurretIronAmount(depositAmount);
        UpdateDepositAmount(0);
        UpdateIronIndicator();

    }

    public void CloseForge()
    {
        forge.CloseMenu();
    }


    public void UpdateIronIndicator()
    {
        playerIronIndicator.text = "Iron Left: " + forge.GetPlayerIron();
        turretIronIndicator.text = "Iron Left: " + forge.GetTurretIron();
    }

    public void TurretUnlocked()
    {
        restrictAccess.SetActive(false);
        turretButton.interactable = true;
        forge.TurretUnlocked();
    }


    public void OnEnable()
    {
        ironAmount = (forge.player.GetResourceAmount(ResourceType.Iron));
        currentIron.text = ironAmount.ToString();
        sliderMaxValue.text = ironAmount.ToString();
        UpdateIronIndicator();
    }
}

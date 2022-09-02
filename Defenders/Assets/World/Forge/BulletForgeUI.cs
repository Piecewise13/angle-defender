using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletForgeUI : MonoBehaviour
{

    public BulletForge forge;
    
    private int woodAmount;
    private int depositAmount;


    public Slider slider;
    private float buttonPercent;


    public TMP_Text currentWood;
    public TMP_Text sliderMaxValue;
    public TMP_Text depositText;

    public TMP_Text playerWoodIndicator;
    public TMP_Text turretWoodIndicator;

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
        UpdateDepositAmount(Mathf.CeilToInt(woodAmount * slider.value));
    }

    public void percentButton(float percent)
    {
        buttonPercent = percent;
        UpdateDepositAmount(Mathf.CeilToInt(woodAmount * percent));
    }

    private void UpdateDepositAmount(int amount)
    {
        depositText.text = "Deposit: " + amount.ToString();
        depositAmount = amount;

    }

    public void DepositPlayer()
    {
        forge.ChangePlayerWoodAmount(depositAmount);
        UpdateDepositAmount(0);
        UpdateWoodIndicator();
    }

    public void DepositTurret()
    {
        forge.ChangeTurretWoodAmount(depositAmount);
        UpdateDepositAmount(0);
        UpdateWoodIndicator();

    }

    public void CloseForge()
    {
        forge.CloseMenu();
    }


    public void UpdateWoodIndicator()
    {
        playerWoodIndicator.text = "Wood Left: " + forge.GetPlayerWood();
        turretWoodIndicator.text = "Wood Left: " + forge.GetTurretWood();
    }

    public void TurretUnlocked()
    {
        restrictAccess.SetActive(false);
        turretButton.interactable = true;
        forge.TurretUnlocked();
    }


    public void OnEnable()
    {
        if (forge.player == null)
        {
            return;
        }
        print(forge.player);
        woodAmount = (forge.player.GetResourceAmount(ResourceType.Wood));
        currentWood.text = woodAmount.ToString();
        sliderMaxValue.text = woodAmount.ToString();
        UpdateWoodIndicator();
    }
}

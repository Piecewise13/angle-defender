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
    public TMP_Text depositButton;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sliderChange()
    {
        updateDeposit(Mathf.CeilToInt(ironAmount * slider.value));
    }

    public void percentButton(float percent)
    {
        buttonPercent = percent;
        updateDeposit(Mathf.CeilToInt(ironAmount * percent));
    }

    private void updateDeposit(int amount)
    {
        depositButton.text = "Deposit: " + amount.ToString();
        depositAmount = amount;

    }

    public void deposit()
    {
        forge.ChangeIronAmount(depositAmount);
        depositAmount = 0;
    }


    public void OnEnable()
    {
        ironAmount = ((int)forge.player.GetResourceAmount(ResourceType.Iron));
        currentIron.text = ironAmount.ToString();
        sliderMaxValue.text = ironAmount.ToString();

    }
}

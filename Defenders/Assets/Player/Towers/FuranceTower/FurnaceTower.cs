using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceTower : TowerParentScript
{
    private bool hasPlayer;
    private bool inMenu = false;



    [Header("Input Vars")]
    public float productionTime;
    private float lastProductionTime;

    public int fuelBurnAmount;
    public int fuelAmount = 0;
    public int woodFuelAmount;
    public int ironFuelAmount;
    public int diamondFuelAmount;


    [Header("Output Vars")]
    public int firePerTick;
    public int fireStored;

    public float playerTransferTime;
    private float lastPlayerTransferTime;
    public int playerTransferAmount;

    public int fuelMax;
    public int soulFireMax;




    //Gameobject Components
    [Space(20)]
    [Header("Effect Gameobjects")]
    public BulletForgeUI bulletForgeUI;
    public ParticleSystem[] eyeFire;
    public ParticleSystem mouthFire;

    // Update is called once per frame
    void Update()
    {

        //Make Fire
        if (fuelAmount > 0)
        {
            if (fireStored < soulFireMax)
            {
                if (productionTime + lastProductionTime < Time.time)
                {

                    fireStored += firePerTick;
                    fuelAmount -= fuelBurnAmount;


                    bulletForgeUI.UpdateSoulFireMeter();
                    bulletForgeUI.UpdateFuelMeter();
                    lastProductionTime = Time.time;
                }
            }
        }





        if (hasPlayer)
        {
            if (Input.GetButtonDown("Use"))
            {
                inMenu = !inMenu;
                bulletForgeUI.gameObject.SetActive(inMenu);
                player.openUIElement(inMenu);
            }

            if (fireStored >= playerTransferAmount)
            {
                if (playerTransferTime + lastPlayerTransferTime < Time.time)
                {
                    player.SetSoulFire(playerTransferAmount);
                    fireStored -= playerTransferAmount;
                    bulletForgeUI.UpdateSoulFireMeter();


                    lastPlayerTransferTime = Time.time;
                }
            }
        }
    }


    public void PlayerEnter(PlayerScript player)
    {
        hasPlayer = true;
        SetPlayer(player);



    }

    public void PlayerExit()
    {
        hasPlayer = false;
    }


    public void DepositResources(ResourceType type, int num)
    {
        switch (type)
        {
            case ResourceType.Wood:
                player.SetResourceAmount(ResourceType.Wood, -num);
                fuelAmount += woodFuelAmount * num;

                break;
            case ResourceType.Iron:
                player.SetResourceAmount(ResourceType.Iron, -num);
                fuelAmount += ironFuelAmount * num;

                break;
            case ResourceType.Diamond:
                player.SetResourceAmount(ResourceType.Diamond, -num);
                fuelAmount += diamondFuelAmount * num;
                break;
        }
    }

    public void CloseMenu()
    {
        inMenu = false;
        bulletForgeUI.gameObject.SetActive(inMenu);
        player.openUIElement(inMenu);
    }

    public void OpenMenu()
    {
        inMenu = true;
        bulletForgeUI.gameObject.SetActive(inMenu);
        player.openUIElement(inMenu);
    }

    public void UpgradeForge(int upgradeAmount)
    {
        firePerTick = Mathf.CeilToInt(firePerTick * (1 + (upgradeAmount * .1f)));
    }

    public bool WithdrawFire(int delta)
    {
        if (fireStored >= delta)
        {
            fireStored -= delta;
            return true;
        }
        return false;

    }

}

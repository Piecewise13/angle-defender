using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForge : MonoBehaviour
{
    private bool playerEnter;
    public PlayerScript player;
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

    [Space(20)]
    [Header("Soul Fire Line")]
    //SoulFire Line Stuff
    public GameObject[] soulFireLine;
    public Renderer[] lineMats;
    public Color activePink;
    public Color activeBlue;
    public Color disable;

    // Update is called once per frame
    void Update()
    {

        //Make Fire
        if (fuelAmount > 0)
        {
            if (fireStored < soulFireMax) {
                if (productionTime + lastProductionTime < Time.time)
                {
                    if (fireStored <= 0)
                    {
                        UpdateTransformers(true);
                        StartSoulFireTubes();
                    }

                    fireStored += firePerTick;
                    fuelAmount -= fuelBurnAmount;

                    if (fuelAmount <= 0)
                    {
                        StopEyeFire();
                    }

                    bulletForgeUI.UpdateFuelMeter();
                    lastProductionTime = Time.time;
                }
            }
        }





        if (playerEnter)
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
                    if (fireStored < playerTransferAmount)
                    {
                        UpdateTransformers(false);
                        StopSoulFireTubes();
                    }


                    lastPlayerTransferTime = Time.time;
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponentInParent<PlayerScript>();
            playerEnter = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerEnter = false;
            
        }
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

        StartEyeFire();
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

    public void TurretUnlocked()
    {
        for (int i = 0; i < soulFireLine.Length; i++)
        {
            soulFireLine[i].SetActive(true);
        }
        StopSoulFireTubes();
    }


    private void StartEyeFire()
    {
        for (int i = 0; i < eyeFire.Length; i++)
        {
            eyeFire[i].Play();
        }
    }

    private void StartSoulFireTubes()
    {
        for (int i = 0; i < lineMats.Length; i++)
        {
            lineMats[i].material.SetColor("_FireBase", activePink);
            lineMats[i].material.SetColor("_FireSecondary", activeBlue);
            lineMats[i].material.SetFloat("_Speed", -.5f);
        }

    }

    private void StopSoulFireTubes()
    {
        for (int i = 0; i < lineMats.Length; i++)
        {
            lineMats[i].material.SetColor("_FireBase", disable);
            lineMats[i].material.SetColor("_FireSecondary", disable);
            lineMats[i].material.SetFloat("_Speed", 0f);
        }
    }

    private void StopEyeFire()
    {
        for (int i = 0; i < eyeFire.Length; i++)
        {
            eyeFire[i].Stop();
        }
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

    private void UpdateTransformers(bool hasFire)
    {
        SoulFireTransformerScript[] script = FindObjectsOfType<SoulFireTransformerScript>();
        foreach (var item in script)
        {
            item.ForgeHasFire(hasFire);
        }
    }

}

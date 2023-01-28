using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceTower : TowerParentScript
{
    private bool hasPlayer;
    private bool inMenu = false;
    private bool inUpgradeMenu = false;


    [Header("Input Vars")]
    [SerializeField] private float defaultOutputSpeed;
    private float outputSpeed;
    private float lastOutputTime;

    [SerializeField] private int defaultFuelBurnAmount;
    private int fuelBurnAmount;

    private int fuelAmount = 0;

    [SerializeField] private int woodFuelAmount;
    [SerializeField] private int ironFuelAmount;
    [SerializeField] private int diamondFuelAmount;


    [Header("Output Vars")]
    public int firePerTick;
    public int fireStored;

    [SerializeField] private int defaultSoulFireOutputAmount;
    private int soulFireOutputAmount;

    public int fuelMax;

    /* UPGRADE PATHS
     * 1. OUTPUT: Increases the size of the soul fire balls and the amount of soul fire
     * 2. SPEED: Increase the production rate of the soul fire and affects the dispenser
     * 3. EFFICIENCY: Decreases the amount of resources needed to produce fire affects the forge and the fire
     */

    //UPGRADE VARS
    private float efficienyMultiplier = 1;
    private float speedMultiplier = 1;
    private float outputMultiplier = 1;


    //Gameobject Components
    [Space(20)]
    [Header("Effect Gameobjects")]
    public BulletForgeUI bulletForgeUI;
    public ParticleSystem forgeFire;
    public Transform fireSpawnPoint;
    public GameObject soulFireBall;
    private Vector3 soulfireBallScale;
    [SerializeField] private float soulFireBallMinScaleFactor;
    [SerializeField] private float soulFireBallMaxScaleFactor;

    [Space(20)]
    [Header("Tower Components")]
    public GameObject forgeModel;
    public GameObject dispenserModel;
    //[Space(10)]




    private Transform[] soulfireSpawnPoints;
    private int dispenserTracker;
    public float launchForce;

    [SerializeField] private Transform dispenserSpawnPoint;

    [SerializeField] private GameObject forgeObject;
    [SerializeField] private GameObject dispenserObject;

    private void Start()
    {
        
        soulfireSpawnPoints = FindSpawnPoints(dispenserObject).ToArray();

        outputSpeed = defaultOutputSpeed;
        fuelBurnAmount = defaultFuelBurnAmount;
        soulfireBallScale = Vector3.one * soulFireBallMinScaleFactor;
        soulFireOutputAmount = defaultSoulFireOutputAmount;
    }


    // Update is called once per frame
    void Update()
    {

        //Make Fire
        if (fuelAmount > 0)
        {
            if (outputSpeed + lastOutputTime < Time.time)
            {
                lastOutputTime = Time.time;
                print("output speed: " + outputSpeed);
                fuelAmount -= fuelBurnAmount;
                DispenseSoulFire();

                //bulletForgeUI.UpdateFuelMeter();
                
            }
        }




        if (towerCamera.isActiveAndEnabled)
        {
            return;
        }
        if (hasPlayer)
        {
            if (Input.GetButtonDown("Use"))
            {
                inMenu = !inMenu;
                bulletForgeUI.gameObject.SetActive(inMenu);
                player.openUIElement(inMenu);
            }
        }
    }


    public override void SetPlayer(PlayerScript player)
    {
        hasPlayer = true;
        base.SetPlayer(player);
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

    public void UpgradeEfficency(float factor)
    {
        efficienyMultiplier = factor;
        fuelBurnAmount = (int)(defaultFuelBurnAmount / factor);
    }

    public void UpgradeSpeed(float factor)
    {
        speedMultiplier = factor;
        outputSpeed = defaultOutputSpeed / speedMultiplier;
    }

    public void UpgradeOutput(float factor, float upgradePercentage)
    {
        soulFireOutputAmount = (int)(defaultSoulFireOutputAmount * factor);
        soulfireBallScale = Vector3.Lerp(soulFireBallMinScaleFactor * Vector3.one,
                                         soulFireBallMaxScaleFactor * Vector3.one,
                                         upgradePercentage);
    }

    public void ChangeForge(GameObject model)
    {
        DestroyImmediate(forgeObject);
        forgeModel = model;
        forgeObject = Instantiate(forgeModel, transform);

        Transform[] spawnpoints = FindSpawnPoints(forgeObject).ToArray();
        fireSpawnPoint = spawnpoints[0];
        //forgeFire.transform.SetParent(fireSpawnPoint);
        //forgeFire.transform.position = Vector3.zero;

        dispenserSpawnPoint = spawnpoints[1];
        ChangeDispenser(dispenserModel);


    }

    public void ChangeDispenser(GameObject model)
    {
        DestroyImmediate(dispenserObject);
        dispenserModel = model;
        dispenserObject = Instantiate(dispenserModel, dispenserSpawnPoint);

        soulfireSpawnPoints = FindSpawnPoints(dispenserObject).ToArray();
        dispenserTracker = 0;
    }

    private void DispenseSoulFire()
    {
        //TODO fix soulfire not colliding and shooting in random directions
        int dispenserIndex = dispenserTracker % soulfireSpawnPoints.Length;
        Transform spawnpoint = soulfireSpawnPoints[dispenserIndex];
        SoulFireBall script = Instantiate(soulFireBall, spawnpoint.position, spawnpoint.rotation).GetComponent<SoulFireBall>();
        script.SetSoulFire(soulFireOutputAmount);
        script.gameObject.transform.localScale = soulfireBallScale;
        Vector3 launchDir = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * launchForce / 5 + Vector3.up * launchForce;
        //Vector3.up + Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f);
        print(launchDir);
        script.LaunchBall(launchDir);
        dispenserTracker++;
    }


    public int GetResourceFuelAmount(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood:
                return woodFuelAmount;
            case ResourceType.Iron:
                return ironFuelAmount;
            case ResourceType.Diamond:
                return diamondFuelAmount;

        }
        return 0;
    }

    public int GetFuelAmount()
    {
        return fuelAmount;
    }


}

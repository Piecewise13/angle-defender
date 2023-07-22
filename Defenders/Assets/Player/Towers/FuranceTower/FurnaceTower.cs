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

    [SerializeField]private int fuelAmount = 0;
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
    public FurnaceUI bulletForgeUI;
    public Transform fireSpawnPoint;
    public GameObject soulFireBall;
    private Vector3 soulfireBallScale;
    [SerializeField] private float soulFireBallMinScaleFactor;
    [SerializeField] private float soulFireBallMaxScaleFactor;

    public Animator anim;

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

                bulletForgeUI.UpdateFuelMeter();
                
            }
        } else
        {
            if(anim != null)
            {
                anim.SetBool("isBurning", false);
            }
        }



        //check that makes it impossible for player to open the furnace menu when the camera is enabled
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
                player.OpenMenu(inMenu);
            }
        }
    }


    public override void SetPlayer(PlayerScript player)
    {
        hasPlayer = true;
        base.SetPlayer(player);
        player.hudScript.DisplayHint(PLAYER_HINT.USE);
    }


    public void PlayerExit()
    {
        hasPlayer = false;
        player.hudScript.StopDisplayingHint();
    }

    //Called by the furnace UI when the player deposits resources into the tower
    public void DepositResources(ResourceType type, int num)
    {
        player.ChangeDiamondAmount(-num);
        fuelAmount += diamondFuelAmount * num;

        lastOutputTime = Time.time;
        if (anim != null)
        {
            anim.SetBool("isBurning", true);
        }
    }

    /* UPGRADE FUNCTIONS */

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

        anim = forgeObject.GetComponent<Animator>();


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
        return diamondFuelAmount;
    }

    public int GetFuelAmount()
    {
        return fuelAmount;
    }


}

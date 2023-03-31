using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealer_Script : TowerParentScript
{

    //STORAGE VARS
    [Header("Storage Vars")]
    public GameObject storageModel;
    public GameObject storageObject;


    //ANTENNA VARS
    [Header("Antenna Vars")]
    public GameObject antennaModel;
    public GameObject antennaObject;
    public Transform antennaSpawnPoint;
    public float wallSearchTime;
    private float lastSeachTime;
    public LayerMask defenseLayer;


    //ROBOT VARS
    public GameObject robotPrefab;
    private List<GameObject> robots = new List<GameObject>();
    private bool hasRobots = true;
    private int maxRobots;
    public Transform[] robotSpawnpoints;
    private int robotSpawnpointTracker;


    //UPGRADE VARS
    [Header("Upgrade Vars")]
    //Range
    [SerializeField] private float defaultRange;
    private float currentRange;

    //Speed
    [SerializeField] private float defaultRobotSpeed;
    private float currentRobotSpeed;

    //Repair Amount
    [SerializeField]private float defaultRepairValue;
    private float currentRepairValue;

    //TOWER VARS
    public List<WallDefenceScript> wallsToHeal = new List<WallDefenceScript>();
    //public Queue<WallDefenceScript> wallsToHeal = new Queue<WallDefenceScript>();

    private void Start()
    {
        currentRange = defaultRange;
        currentRepairValue = defaultRange;
        currentRobotSpeed = defaultRobotSpeed;

        UpdateWallsToHeal();
        maxRobots = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //If wallSearchTime has past, search for damaged alls in range and add them to updateWallsToHeal
        if (lastSeachTime + wallSearchTime < Time.time)
        {
            UpdateWallsToHeal();
            lastSeachTime = Time.time;
        }

        //If tower doesn't have any avalible robots return
        if (!hasRobots)
        {
            return;
        }

        //if wallsToHeal contains nothing then return
        if (wallsToHeal.Count == 0)
        {
            return;
        }

        //for each wall in wallsToHeal, send out a robot if we have robots
        if (wallsToHeal.Count > 0)
        {
            SendOutRobot(wallsToHeal[0]);
        }
        /*
        foreach (var wall in wallsToHeal)
        {
            if (hasRobots)
            {
                SendOutRobot(wall);
            }
        }
        */
    }

    //Creates a robot, initalizes its values, adds it to robots
    public void SendOutRobot(WallDefenceScript wall)
    {
        print("sending out robots");
        int index = robotSpawnpointTracker % robotSpawnpoints.Length;

        GameObject robot = Instantiate(robotPrefab, robotSpawnpoints[index].position, robotSpawnpoints[index].rotation);
        
        RobotScript script = robot.GetComponent<RobotScript>();

        print(script);
        script.InitializeRobot(this, wall, robotSpawnpoints[index]);
        script.SetSpeed(currentRobotSpeed);
        script.SetRepairAmount(currentRepairValue);

        robots.Add(robot);

        wallsToHeal.Remove(wall);
        wallsToHeal.Add(wall);

        robotSpawnpointTracker++;

        hasRobots = robots.Count < maxRobots;
    }

    //called by the robot when the robot is returned to the tower
    public void ReturnRobot(GameObject robot)
    {
        robots.Remove(robot);
        Destroy(robot);
        hasRobots = robots.Count <= maxRobots;
    }


    //Called by the wall when the wall is damaged
    public void WallNeedsHealing(WallDefenceScript wall)
    {
        if (wallsToHeal.Contains(wall))
        {
            return;
        }

        wallsToHeal.Add(wall);
    }

    //Called by the wall when the wall is either destroyed or fully healed
    public void EndHealingService(WallDefenceScript wall)
    {
        wallsToHeal.Remove(wall);
    }

    //Searches for walls in a specificed radius which needs healing then lets them know they are in range of a wall healer
    public void UpdateWallsToHeal()
    {
        Collider[] overlapDefenses = Physics.OverlapSphere(transform.position, currentRange, defenseLayer);
        print(overlapDefenses.Length);

        foreach (var item in overlapDefenses)
        {
            try
            {
                WallDefenceScript wall;
                wall = item.gameObject.GetComponentInParent<WallDefenceScript>();
                wall.InWallHealerRange(this);
            }
            catch
            {

            }
        }
    }

    /* UPGRADE FUNCTIONS */

    //Upgrades the speed of the robots
    public void UpgradeSpeed(float factor)
    {
        currentRobotSpeed = defaultRobotSpeed * factor;
        
    }

    //upgrades the search range for the tower
    public void UpgradeRange(float factor)
    {
        currentRange = defaultRange * factor;

        UpdateWallsToHeal();
    }

    //upgrades the amount of health restored by each robot
    public void UpgradeRepairAmount(float factor)
    {
        currentRepairValue = defaultRepairValue * factor;
    }




    /* CHANGE GAME OBJECT COMPONENTS */
    //changes the storage when speed is upgraded
    public void ChangeStorage(GameObject model)
    {
        DestroyImmediate(storageObject);
        storageModel = model;
        storageObject = Instantiate(storageModel, transform);

        List<Transform> spawnpoints = FindSpawnPoints(storageObject);
        antennaSpawnPoint = spawnpoints[0];
        robotSpawnpoints = spawnpoints.GetRange(1, spawnpoints.Count - 1).ToArray();
        robotSpawnpointTracker = 0;

        ChangeAntenna(antennaModel);
    }

    //Change the antenna when range is upgraded
    public void ChangeAntenna(GameObject model)
    {
        DestroyImmediate(antennaObject);
        antennaModel = model;
        antennaObject = Instantiate(antennaModel, antennaSpawnPoint.position, antennaSpawnPoint.rotation);
    }

    //Change the robot when repair amount is upgraded
    public void ChangeRobot(GameObject robot)
    {
        robotPrefab = robot;
        foreach (var item in robots)
        {
            Destroy(item);
        }
    }


    public void IncreaseMaxRobots()
    {
        maxRobots++;
    }
}

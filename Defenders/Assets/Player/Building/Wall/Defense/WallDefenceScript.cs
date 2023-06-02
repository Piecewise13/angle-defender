using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallDefenceScript : MonoBehaviour, Damageable
{
    [Header("Resource")]
    public ResourceType type;
    public static Dictionary<ResourceType, int> cost = new Dictionary<ResourceType, int>();

    [Space(20)]
    [Header("Wall Vars")]
    public GameObject[] wallObjects;
    private int currentWall;
    
    [Space(20)]
    [Header("Health Vars")]
    public float maxHealth;
    private bool needsRepair;
    public float health { get; set; }

    public bool isDead { get; set; }


    [Header("Component Vars")]
    public GameObject wallHolder;
    [SerializeField]private GameObject wallObject;
    private NavMeshObstacle obstacle;
    public Collider collide;
    private PlayerScript player;
    private bool hasPlayer = false;

    [Space(20)]
    [Header("RepairVars")]
    public float repairTime;
    private float lastRepairTime;
    public float rebuildDelay;
    private float startRebuildTime;

    private bool hasWallHealer;
    private WallHealer_Script wallHealer;

    //MISC VARS
    public static PlayerDataMangerScript dataManager;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        obstacle = GetComponentInChildren<NavMeshObstacle>();
        cost.Clear();
        cost.Add(ResourceType.Wood, 10);
        cost.Add(ResourceType.Iron, 0);
        cost.Add(ResourceType.Diamond, 0);

        if (dataManager == null)
        {
            dataManager = FindObjectOfType<PlayerDataMangerScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPlayer)
        {
            if (Input.GetButtonDown("Use"))
            {
                lastRepairTime = Time.time;
                startRebuildTime = Time.time;

            }

            //TODO implement new system that uses the utility feature to upgrade the wall
            /*
            if (Input.GetButton("Use"))
            {
                if (health < maxHealth)
                {
                    if (lastRepairTime + repairTime < Time.time)
                    {

                        if (CanAfford())
                        {
                            if (Repair())
                            {
                                ChargePlayer();
                            }

                        }
                        else
                        {
                            print("Can't Afford");
                            //PLAY SOUND
                        }
                        lastRepairTime = Time.time;
                    }
                }
            }
            */
        }
    }

    public bool Repair(float repairAmount)
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
            ChangeWallObject();

            wallHealer.EndHealingService(this);

            return false;
        }
        else
        {
           
            health += repairAmount;
            ChangeWallObject();
            return true;
        }

    }

    private bool CanAfford()
    {

        for (int i = 0; i < (int)ResourceType.Count; i++)
        {
            if (player.GetResourceAmount((ResourceType)i) < WallDefenceScript.cost[(ResourceType)i])
            {

                return false;
            }
        }

        return true;
    }


    private void ChargePlayer()
    {
        for (int i = 0; i < (int)ResourceType.Count; i++)
        {
            player.SetResourceAmount((ResourceType)i, -WallDefenceScript.cost[(ResourceType)i]);
        }
    }



    public void Rebuild()
    {
        collide.enabled = false;
        //wallHolder.SetActive(true);
        obstacle.enabled = true;
        isDead = false;
    }

    public void InWallHealerRange(WallHealer_Script script)
    {
        wallHealer = script;
        hasWallHealer = true;
    }

    public void LostWallHealer()
    {
        wallHealer = null;
        hasWallHealer = false;
    }

    public void PlayerEnter(PlayerScript player)
    {
        this.player = player;
        hasPlayer = true;
    }

    public void PlayerExit()
    {
        this.player = null;
        hasPlayer = false;
    }

    public void Death()
    {
        health = 0f;
        //print(wallMesh.activeSelf);
        //wallHolder.SetActive(false);
        collide.enabled = false;
        obstacle.enabled = false;
        isDead = true;
        if (wallHealer != null)
        {
            wallHealer.EndHealingService(this);
        }
        dataManager.RemoveDefenseLocation(gameObject.transform.position);
        Destroy(gameObject);
    }

    public void ChangeWallObject()
    {
        
        float healthPercentage = health / maxHealth;

        int index = Mathf.CeilToInt(Mathf.Lerp(wallObjects.Length - 1, 0, healthPercentage));
        //int index = Mathf.Clamp(Mathf.CeilToInt((1 - healthPercentage) * (wallObjects.Length)), 0, wallObjects.Length - 1);

        if (currentWall != index)
        {
            Destroy(wallObject);
            if (index < 0)
            {
                return;
            }
            wallObject = Instantiate(wallObjects[index], wallHolder.transform);
            currentWall = index;
        }
    }

    public bool GetNeedsRepair()
    {
        return needsRepair;
    }

    public void GiveDamage(float damage, Collider hitCollider, out float damageGiven, out bool crit)
    {
        GiveDamage(damage);
        damageGiven = damage;
        crit = false;
    }

    public void GiveDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;

            if (hasWallHealer)
            {
                wallHealer.WallNeedsHealing(this);
            }

            ChangeWallObject();

            //print(wallMesh.activeSelf);
            //print("Wall taking damage health: " + health);
            if (health <= 0.0f)
            {
                Death();
            }
        }
    }
}

public interface repairable
{
    float health { get; set; }
    float maxHealth { get; set; }
    ResourceType type { get; set; }
    float cost { get; set;}
    void repair();
}
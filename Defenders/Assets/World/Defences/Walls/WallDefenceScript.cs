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
    public float repairAmount;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        obstacle = GetComponentInChildren<NavMeshObstacle>();
        cost.Clear();
        cost.Add(ResourceType.Wood, 10);
        cost.Add(ResourceType.Iron, 0);
        cost.Add(ResourceType.Diamond, 0);
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
        }
    }

    public bool Repair()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
            ChangeWallObject();
            return false;
        }
        else
        {
            health += repairAmount;
            ChangeWallObject();

            return true;
        }
        /*
        if (health <= 0)
        {
            /*
            if (startRebuildTime + rebuildDelay < Time.time)
            {
                Rebuild();
                health += repairAmount;
                ChangeWallObject();
                return true;
            }
           
        }
        else
        {


        }
      */

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

    public void TakeDamage(float damage, Collider hitCollider)
    {
        if (!isDead)
        {
            health -= damage;
            ChangeWallObject();

            //print(wallMesh.activeSelf);
            //print("Wall taking damage health: " + health);
            if (health <= 0.0f)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        health = 0f;
        //print(wallMesh.activeSelf);
        //wallHolder.SetActive(false);
        collide.enabled = false;
        obstacle.enabled = false;
        isDead = true;

    }

    public void ChangeWallObject()
    {
        
        float healthPercentage = health / maxHealth;

        int index = Mathf.CeilToInt(Mathf.Lerp(wallObjects.Length - 1, 0, healthPercentage));
        //int index = Mathf.Clamp(Mathf.CeilToInt((1 - healthPercentage) * (wallObjects.Length)), 0, wallObjects.Length - 1);

        print("Health %: " + healthPercentage + ", index: " + index);

        if (currentWall != index)
        {
            Destroy(wallObject);
            print("trying to get: " + index);
            if (index < 0)
            {
                return;
            }
            wallObject = Instantiate(wallObjects[index], wallHolder.transform);
            currentWall = index;
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
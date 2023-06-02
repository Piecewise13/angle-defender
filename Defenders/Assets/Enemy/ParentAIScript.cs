using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public abstract class ParentAIScript : MonoBehaviour, Damageable
{
    protected NavMeshAgent agent;
    protected float speed;
    protected static EggScript eggScript;
    protected static GameObject egg;
    protected static ResourceSpawner resourceSpawner;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float attackDamage;
    protected WallDefenceScript targetWall;
    protected NavMeshPath path;
    //protected static Object soulFireBallPrefab;
    [SerializeField] private int soulFireWorth;

    public MasterAI masterAI;
    public static WallDefenceScript[] walls;
    protected bool inLure;
    protected Vector3 lureDestination;
    [SerializeField]protected bool canBeLured;
    protected bool atWall;

    private PlayerDataMangerScript playerDataManager;


    public float health { get; set; }
    public bool isDead { get ; set ; }

    [SerializeField] private float damageMultiplier;


    // Start is called before the first frame update
    public void Start()
    {
        health = maxHealth;
        resourceSpawner = FindObjectOfType<ResourceSpawner>();
        eggScript = FindObjectOfType<EggScript>();
        egg = eggScript.transform.root.gameObject;

        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        walls = FindObjectsOfType<WallDefenceScript>();
        path = new NavMeshPath();
        //soulFireBallPrefab = Resources.Load("SoulFireBall");
        masterAI = FindObjectOfType<MasterAI>();
        playerDataManager = FindObjectOfType<PlayerDataMangerScript>();
    }



    protected WallDefenceScript GetClosestWall(Vector3 refPos)
    {
        int closestIndex = 0;
        float distance = float.MaxValue;
        for (int i = 0; i < walls.Length; i++)
        {
            float currentDist = Vector3.Distance(refPos, walls[i].transform.position);
            if (currentDist < distance)
            {
                
                closestIndex = i;
                distance = currentDist;
            }
        }
        return walls[closestIndex];
    }

    protected WallDefenceScript GetRandomWall()
    {
        if (targetWall != null)
        {
            return targetWall;
        }
        
        return walls[Random.Range(0, walls.Length)];
    }


    public static void UpdateWalls()
    {
        walls = FindObjectsOfType<WallDefenceScript>();
    }

    public virtual void AtWall(WallDefenceScript wall)
    {
        atWall = true;
        targetWall = wall;
    }

    public virtual void LeaveWall()
    {
        atWall = false;
    }

    public void Freeze(float time)
    {
        agent.speed = speed * .5f;
        Invoke("EndFreeze", time);
    }

    public void EndFreeze()
    {
        agent.speed = speed;
    }



    public void Lure(Vector3 loc)
    {
        if (canBeLured)
        {
            inLure = true;
            agent.destination = loc;
        }

    }

    public void EndLure()
    {
        inLure = false;
    }

    protected void GoToEgg()
    {
        if (egg == null)
        {
            return;
        }

        if (agent == null)
        {
            return;
        }
        agent.SetDestination(egg.transform.position);
    }

    public virtual void Death()
    {
        //Instantiate(soulFireBallPrefab, transform.position + Vector3.up * 2f, transform.rotation);
        playerDataManager.GiveSoulFire(soulFireWorth);
        masterAI.Enemy_Killed(gameObject);
        Destroy(gameObject);
    }

    public void ReachedEgg() {
        masterAI.Enemy_Killed(gameObject);
        Destroy(gameObject);
        //START A COUROTINE TO MAKE COOL EFFECTS AND SHIT
    }

    public virtual void GiveDamage(float damage, Collider hitCollider, out float damageGiven, out bool crit)
    {
        crit = hitCollider.tag == "Crit";
        damageGiven = damage;
        if (crit)
        {
            damageGiven = damage * damageMultiplier;
        }
        GiveDamage(damage);
        
    }

    public virtual void GiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {

            Death();
        }
    }
}

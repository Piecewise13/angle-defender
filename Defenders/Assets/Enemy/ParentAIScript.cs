using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public abstract class ParentAIScript : MonoBehaviour, Damageable
{
    protected NavMeshAgent agent;
    protected static EggScript eggScript;
    protected static GameObject egg;
    protected static ResourceSpawner resourceSpawner;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float attackDamage;
    protected WallDefenceScript targetWall;
    protected NavMeshPath path;
    protected static Object soulFireBallPrefab;

    public MasterAI masterAI;
    public static WallDefenceScript[] walls;
    protected bool inLure;
    protected Vector3 lureDestination;
    [SerializeField]protected bool canBeLured;
    protected bool atWall;




    public float health { get; set; }
    public bool isDead { get ; set ; }



    // Start is called before the first frame update
    public void Start()
    {
        health = maxHealth;
        resourceSpawner = FindObjectOfType<ResourceSpawner>();
        eggScript = FindObjectOfType<EggScript>();
        egg = eggScript.transform.root.gameObject;

        agent = GetComponent<NavMeshAgent>();

        walls = FindObjectsOfType<WallDefenceScript>();
        path = new NavMeshPath();
        soulFireBallPrefab = Resources.Load("SoulFireBall");
        masterAI = FindObjectOfType<MasterAI>();
    }



    public WallDefenceScript GetClosestWall(Vector3 refPos)
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

    public WallDefenceScript GetRandomWall()
    {
        if (targetWall != null)
        {
            return targetWall;
        }

        return walls[Random.Range(0, walls.Length)];


        //int closestIndex = 0;
        //float distance = float.MaxValue;
        //for (int i = 0; i < walls.Length; i++)
        //{
        //    float currentDist = Vector3.Distance(transform.position, walls[i].transform.position);
        //    if (currentDist < distance)
        //    {

        //        closestIndex = i;
        //        distance = currentDist;
        //    }
        //}
        //return walls[closestIndex];
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

    public virtual void TakeDamage(float damage, Collider hitCollider)
    {
        health -= damage;
        if (health <= 0f)
        {
            
            Death();
        }
    } 

    public virtual void Death()
    {
        Instantiate(soulFireBallPrefab, transform.position + Vector3.up * 2f, transform.rotation);
        masterAI.Enemy_Killed(gameObject);
        Destroy(gameObject);
    }

    public void ReachedEgg() {
        print("Reached Egg");
        Destroy(gameObject);
        //START A COUROTINE TO MAKE COOL EFFECTS AND SHIT
    }

}

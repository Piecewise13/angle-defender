using System.Collections;
using System.Collections.Generic;
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

    public static WallDefenceScript[] walls;

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


    }

    public WallDefenceScript GetRandomWall()
    {
        int index = (int)(Random.value * walls.Length);
        return walls[index];
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


    public static void UpdateWalls()
    {
        walls = FindObjectsOfType<WallDefenceScript>();
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
        Destroy(gameObject);
    }

    public void ReachedEgg() {
        print("Reached Egg");
        Destroy(gameObject);
        //START A COUROTINE TO MAKE COOL EFFECTS AND SHIT
    }

}

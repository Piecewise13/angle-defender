using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicResourceCollector : ParentAIScript
{

    #region OldVars
    private GameObject targetResource;
    public ResourceType targetType;
    private Damageable targetScript;


    public float headDamageMultiplier;

    public float resourceCollectionDelay;
    private float lastAttackTime;
    private bool shouldCollect;
    public ResourceType collectedResource;
    public int collectedAmount;

    public GameObject woodIngot;
    public GameObject ironIngot;
    public GameObject diamondIngot;

    [SerializeField] private GameObject tool;
    private MeshFilter toolMesh;

    public MeshFilter axeMesh;
    public MeshFilter pickaxeMesh;

    private Animator anim;
    float timer;
    #endregion


    static MonsterSpawnScript[] spawns;
    private int monsterSpawnIndex;




    //test var
    

    // Start is called before the first frame update
    new void Start()
    {

        base.Start();


        toolMesh = tool.GetComponentInChildren<MeshFilter>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        spawns = FindObjectsOfType<MonsterSpawnScript>();
        monsterSpawnIndex = Random.Range(0, spawns.Length);
        AssignTarget(spawns[monsterSpawnIndex].neededResource);
    }




    #region OldCode

    private void Update()
    {
        if (agent.remainingDistance <= 1f && shouldCollect && targetResource != null)
        {

            anim.SetBool("isCollecting", shouldCollect);
            if (lastAttackTime + resourceCollectionDelay < Time.time)
            {
                targetScript.GiveDamage(attackDamage);
                lastAttackTime = Time.time;
            }
        }


        if (shouldCollect && targetResource == null)
        {
            if (agent.remainingDistance <= 1f)
            {
                if (timer >= 5f)
                {

                    AssignTarget(targetType);

                }
                timer += Time.deltaTime;
            }
        }
    }



    public void AssignTarget(ResourceType type)
    {

        targetType = type;
        shouldCollect = true;
        timer = 0f;
        //targetResource = resourceSpawner.GetClostestResourceOfType(transform.position, type);


        targetScript = targetResource.GetComponentInChildren<Damageable>();
        agent.destination = (targetResource.transform.position);
        switch (type)
        {
            case (ResourceType.Wood):
                toolMesh.sharedMesh = axeMesh.sharedMesh;
                break;
            case (ResourceType.Iron):
                toolMesh.sharedMesh = pickaxeMesh.sharedMesh;
                break;

        }
    }

    public void collectResource(ResourceType type, int amount)
    {
        //spawnresource in bag
        collectedResource = type;
        collectedAmount = amount;
        ReturnToBank();


        //return to bank
    }

    public void ReturnToBank()
    {
        agent.destination = spawns[monsterSpawnIndex].transform.position;
        shouldCollect = false;
        anim.SetBool("isCollecting", shouldCollect);
    }
    #endregion





    public new void Death()
    {
        if (collectedAmount > 0)
        {
            switch (collectedResource)
            {
                case ResourceType.Wood:
                    Instantiate(woodIngot, transform.position, Quaternion.Euler(0f,0f,0f));
                    break;
                case ResourceType.Iron:
                    Instantiate(ironIngot, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    break;
                case ResourceType.Diamond:
                    Instantiate(diamondIngot, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    break;
            }

        }

        //spawn death stuff
        print("death");
        Destroy(gameObject);
    }



    public new void TakeDamage(float damage, Collider hitCollider)
    {
        
        if (hitCollider.gameObject.tag.Equals("Head"))
        {
            health -= damage * headDamageMultiplier;
        }
        else //if (hitCollider.gameObject.tag.Equals("Body"))
        {
            health -= damage;   
        }
        
        if (health <= 0f)
        {
            
            Death();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicResourceCollector : ParentAIScript
{

    private GameObject targetResource;
    private Damageable targetScript;
    
    
    public float headDamageMultiplier;

    public float resourceCollectionDelay;
    private float lastAttackTime;
    private bool shouldCollect;
    public float attackDamage;
    public GameObject resourceBank;
    public ResourceType collectedResource;
    public float collectedAmount;

    public GameObject woodIngot;
    public GameObject ironIngot;
    public GameObject diamondIngot;

    [SerializeField] private GameObject tool;
    private MeshFilter toolMesh;

     public MeshFilter axeMesh;
     public MeshFilter pickaxeMesh;

    private Animator anim;
    float timer;


    //test var
    public ResourceType targetType;

    // Start is called before the first frame update
    void Start()
    {
        
        initValues();


        toolMesh = tool.GetComponentInChildren<MeshFilter>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        assignTarget(targetType);

        
    }

    private void Update()
    {
        if (agent.remainingDistance <= 1f && shouldCollect && targetResource != null)
        {
            
            anim.SetBool("isCollecting", true);
            if (lastAttackTime + resourceCollectionDelay < Time.time)
            {
                targetScript.takeDamage(attackDamage, null);
                lastAttackTime = Time.time;
            }
        }


        if (shouldCollect && targetResource == null)
        {
            if (agent.remainingDistance <= 1f)
            {
                if (timer >= 5f)
                {

                    assignTarget(targetType);

                }
                timer += Time.deltaTime;
            }
        }
    }

    public GameObject findClosestResource(ResourceType type)
    {
        GameObject currentClosest = null;
        float currentDistance = Mathf.Infinity;

        
        foreach (GameObject resource in resourceSpawner.resources)
        {
            
            ResourceScript resourceScript = resource.GetComponentInChildren<ResourceScript>();
            if (resourceScript.resource.Equals(type))
            {
                
                float distance = Vector3.Distance(transform.position, resource.transform.position);
                if (currentDistance > distance)
                {
                    
                    currentClosest = resource;
                    currentDistance = distance;
                } 
            }
        }
       
        return currentClosest;
    }


    public void assignTarget(ResourceType type)
    {
        
        shouldCollect = true;
        timer = 0f;
        targetResource = findClosestResource(type);

        targetType = type;
        targetScript = targetResource.GetComponentInChildren<Damageable>();
        agent.SetDestination(targetResource.transform.position);
        switch (type)
        {
            case(ResourceType.Wood):
                toolMesh.sharedMesh = axeMesh.sharedMesh;
                break;
            case (ResourceType.Iron):
                toolMesh.sharedMesh = pickaxeMesh.sharedMesh;
                break;

        }
    }

    public void collectResource(ResourceType type, float amount)
    {
        //spawnresource in bag
        collectedResource = type;
        collectedAmount = amount;
        shouldCollect = false;
        ReturnToBank();
        

        //return to bank
    }

    public void ReturnToBank()
    {
        agent.SetDestination(resourceBank.transform.position);
        shouldCollect = false;
        anim.SetBool("isCollecting", false);
    }

    



    public override void death()
    {
        print(collectedAmount);
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


    public override void reachedEgg()
    {
        throw new System.NotImplementedException();
    }

    public override void takeDamage(float damage, Collider hitCollider)
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
            
            death();
        }
    }
}
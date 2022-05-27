using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LungerScript : PlayerBasedAIParent
{

    public float updatePlayerLocDelay;
    private float lastUpdatePlayerLoc;

    [Header("Lunge Vars")]
    [SerializeField] private float lungeDist;
    [SerializeField] private float maxLungeTime;
    [SerializeField] private float maxHeight;


    public LayerMask wall;

    [Header("Damage")]
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    private float lastAttack;
    private bool isAttacking;
    private Damageable attackTarget;

    private Rigidbody rb;

    //for testing only
    [SerializeField]private Transform playerTransform;

    private bool isLunge;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        player = ClosestPlayer(transform.position);
        
        playerTransform = player.transform;
        rb = GetComponentInChildren<Rigidbody>();
        //initValues();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (updatePlayerLocDelay + lastUpdatePlayerLoc < Time.time)
        {
            if (agent.isActiveAndEnabled)
            {
                lastUpdatePlayerLoc = Time.time;
                print("new path");
                UpdatePath();
            }
            
            
        }

        if (isAttacking)
        {
            
            if (lastAttack + attackDelay < Time.time)
            {
                
                //attackTarget.takeDamage(damage, null);
                
                lastAttack = Time.time;
                print("attack");

            }
        }
    }

    private void UpdatePath()
    {
        NavMeshPath path = new NavMeshPath();
        //print(agent);
        agent.CalculatePath(playerTransform.position, path);
        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, egg.transform.position, out hit, wall))
            {
                agent.destination = hit.transform.position;

            }
        } else
        {
            agent.destination = playerTransform.position;
        }
    }



    



    IEnumerator lungeAction(Vector3 loc)
    {
        
        agent.enabled = false;

        float distance = Vector3.Distance(transform.position, loc);
        float timer = Mathf.Lerp(0f, maxLungeTime, distance / lungeDist);

        transform.LookAt(loc, Vector3.up);
        //anim start lunge
        yield return new WaitForSeconds(timer);
        //launch

        //Vector3 forceVector = transform.position - loc;
        //float force = Mathf.Lerp(0, lungeForce, distance / lungeDist);
        //forceVector += Vector3.up  * -maxHeight;

        Vector3 forceVector = playerTransform.position - transform.position;

        rb.AddForce(forceVector + (Vector3.up * maxHeight), ForceMode.Impulse);
        


        yield return new WaitForSeconds(1f);
        
        agent.enabled = true;
        isLunge = false;

        yield return null;



    }


    public override void death()
    {
        Destroy(gameObject);
    }

    public override void reachedEgg()
    {
        throw new System.NotImplementedException();
    }

    public override void takeDamage(float damage, Collider hitCollider)
    {
        health -= damage;
        if (health <= 0)
        {
            death();
        }
    }

    public void StartAttacking(Damageable target)
    {
        attackTarget = target;
        isAttacking = true;
    }

    public void StopAttacking()
    {
        isAttacking = false;
    }


    public override void PlayerFound(PlayerScript player)
    {
        if (isLunge)
            return;
        this.player = player;
        isLunge = true;
        playerTransform = player.transform;
        StartCoroutine(lungeAction(playerTransform.position));

        
    }   
    
}

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
    [SerializeField] private float attackDelay;
    private float lastAttack;
    private bool isAttacking;
    private Damageable attackTarget;

    private Rigidbody rb;
    private Animator anim;

    //for testing only
    [SerializeField]private Transform playerTransform;

    [SerializeField]private bool isLunge;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        player = ClosestPlayer(transform.position);
        
        playerTransform = player.transform;
        rb = GetComponentInChildren<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        //initValues();
        agent = GetComponent<NavMeshAgent>();

        InvokeRepeating("UpdatePath", 2f, 10f);
    }

    // Update is called once per frame
    void Update()
    {

        if (isAttacking)
        {
            
            if (lastAttack + attackDelay < Time.time)
            {
                
                player.TakeDamage(attackDamage, null);
                
                lastAttack = Time.time;
                print("attack");

            }
        }
        if (agent.isActiveAndEnabled && agent.remainingDistance < 1f)
        {
            anim.SetBool("isWalking", false);
        }
    }


    private void UpdatePath()
    {
        if (agent.isActiveAndEnabled)
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
            anim.SetBool("isWalking", true);
        }
    }


    private void CheckForPlayer()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, playerRange, playerMask);

        if (players.Length >= 1)
        {
            this.player = players[0].GetComponentInParent<PlayerScript>();
            PlayerFound(player);
        }
    }
    



    IEnumerator lungeAction(Vector3 loc)
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isLunging", true);
        agent.enabled = false;

        float distance = Vector3.Distance(transform.position, loc);
        float timer = Mathf.Lerp(0f, maxLungeTime, distance / lungeDist);

        transform.LookAt(loc, Vector3.up);
        //anim start lunge
        yield return new WaitForSeconds(2.8F);
        //launch

        //Vector3 forceVector = transform.position - loc;
        //float force = Mathf.Lerp(0, lungeForce, distance / lungeDist);
        //forceVector += Vector3.up  * -maxHeight;
        rb.isKinematic = false;

        Vector3 forceVector = playerTransform.position - transform.position;

        rb.AddForce(forceVector + (Vector3.up * maxHeight), ForceMode.Impulse);


        anim.SetBool("isLunging", false);
        yield return new WaitForSeconds(1f);
        
        


        agent.enabled = true;
        UpdatePath();
        rb.isKinematic = true;


        yield return new WaitForSeconds(5f);
        isLunge = false;
        yield return null;



    }



    public override void Death()
    {
        Destroy(gameObject);
    }

    public override void ReachedEgg()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float damage, Collider hitCollider)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
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

    public override void PlayerLost(PlayerScript player)
    {
    }

}

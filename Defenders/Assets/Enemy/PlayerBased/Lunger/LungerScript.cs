using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LungerScript : PlayerBasedAIParent
{


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

    [SerializeField]private bool isLunge;
    private bool hasPlayer;
    public float lungeTime;
    public float startLungeTime;

    public float lungeWaitTime;
    private float lastLungeAttackTime;


    public float searchTime;
    private float lastSearchTime;

    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        health = maxHealth;
        player = GetClosestPlayer();
        
        //playerTransform = player.transform;
        rb = GetComponentInChildren<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
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

        if (searchTime + lastSearchTime < Time.time)
        {
            UpdatePath();
        }


        if (agent.hasPath)
        {
            if (hasPlayer)
            {
                if (agent.remainingDistance < lungeDist)
                {
                    if (lastLungeAttackTime + lungeWaitTime < Time.time)
                    {
                        print(agent.remainingDistance);
                        StartLunge();
                    }

                }

            }
        }


        if (isLunge)
        {
            if (startLungeTime + lungeTime < Time.time)
            {
                EndLunge();

            }
        }

    }


    private void UpdatePath()
    {
        if (agent.isActiveAndEnabled)
        {
            player = GetClosestPlayer();

            NavMeshPath path = new NavMeshPath();
            //print(agent);
            agent.CalculatePath(player.transform.position, path);
            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                print("has pl;ayer");
                agent.destination = player.transform.position;
                hasPlayer = true;
            }
            else
            {
                agent.CalculatePath(egg.transform.position, path);
                if (agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    agent.destination = egg.transform.position;
                }
                else
                {
                    agent.destination = GetClosestWall().transform.position;
                }

            }

            anim.SetBool("isWalking", true);
        }
        lastSearchTime = Time.time;
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

    public void StartLunge()
    {
        agent.enabled = false;
        hasPlayer = false;
        anim.SetBool("isWalking", false);
        anim.SetTrigger("isLunge");
        LookAtPoint(player.transform.position, 1f);
    }

    public void Lunge()
    {
        startLungeTime = Time.time;
        print("lunge that bitch");
        isLunge = true;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        float timer = Mathf.Lerp(0f, maxLungeTime, distance / lungeDist);
        transform.LookAt(player.transform.position, Vector3.up);

        rb.isKinematic = false;

        Vector3 forceVector = player.transform.position - transform.position;

        rb.AddForce(forceVector + (Vector3.up * maxHeight), ForceMode.Impulse);

    }

    public void EndLunge()
    {
        rb.isKinematic = false;
        isLunge = false;
        agent.enabled = true;
        UpdatePath();

    }

    public IEnumerator LookAtPoint(Vector3 point, float dur)
    {
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(point - transform.position);
        float t = 0f;
        while (t < dur)
        {
            transform.rotation = Quaternion.Slerp(start, end, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
    }





    //public override void PlayerFound(PlayerScript player)
    //{
    //    if (isLunge)
    //        return;
    //    this.player = player;
    //    isLunge = true;
    //    StartCoroutine(lungeAction(player.transform.position));
    //}

    public override void PlayerLost(PlayerScript player)
    {
    }

    public override void PlayerFound(PlayerScript player)
    {
    }
}

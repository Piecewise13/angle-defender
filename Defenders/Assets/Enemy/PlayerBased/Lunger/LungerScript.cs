using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LungerScript : PlayerBasedAIParent
{


    [Header("Lunge Vars")]
    [SerializeField] private float lungeDist;
    [SerializeField] private bool isLunge;
    private bool hasPlayer;
    public float lungeTime;
    public float lungeSpeed;
    public float startLungeTime;
    private Vector3 lungeTarget;
    public float lungeWaitTime;
    private float lastLungeAttackTime;


    public LayerMask wall;

    [Header("Damage")]
    [SerializeField] private float attackDelay;
    private float lastAttack;
    private bool isAttacking;
    private Damageable attackTarget;

    private Rigidbody rb;
    private Animator anim;




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
            agent.isStopped = true;
            if (lastAttack + attackDelay < Time.time)
            {
                
                attackTarget.TakeDamage(attackDamage, null);
                lastLungeAttackTime = Time.time;
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
                        StartLunge();
                    }

                }

            }
        }


        if (isLunge)
        {
            float distance = Vector3.Distance(transform.position, lungeTarget);
            if (distance < 1f)
            {
                EndLunge();
            }
            transform.position = Vector3.MoveTowards(transform.position, lungeTarget, lungeSpeed * Time.deltaTime);
        }

    }


    private void UpdatePath()
    {
        lastSearchTime = Time.time;

        anim.SetBool("isWalking", true);
        //figure out if there is a path to player
        player = GetClosestPlayer();
        if (player != null)
        {
            agent.CalculatePath(player.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetPath(path);
                hasPlayer = true;
                return;
            }
        }

        agent.CalculatePath(egg.transform.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetPath(path);
            return;
        }
        targetWall = GetRandomWall();
        agent.destination = targetWall.transform.position;
    }



    public void StartAttacking(Damageable target)
    {
        attackTarget = target;
        isAttacking = true;
    }

    public void StopAttacking()
    {
        attackTarget = null;
        isAttacking = false;
        agent.isStopped = false;
    }

    public void StartLunge()
    {
        agent.enabled = false;
        hasPlayer = false;
        anim.SetBool("isWalking", false);
        anim.SetBool("isLunge", true);
        
        LookAtPoint(player.transform.position, 1f);
    }

    public void Lunge()
    {
        print("start lunging");
        startLungeTime = Time.time;
        lungeTarget = player.transform.position;
        isLunge = true;
        transform.LookAt(lungeTarget, Vector3.up);

    }

    public void EndLunge()
    {
        isLunge = false;
        agent.enabled = true;
        lastLungeAttackTime = Time.time;
        anim.SetBool("isLunge", false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyScript : ParentAIScript
{
    public float attackTime;
    private float timeLastAttack;
    private Damageable attackTarget;
    private GameObject atttackTargetGO;

    private Collider hitCollider;
    public LayerMask wall;

    [SerializeField] private float searchTime;
    private float lastSearchTime;
    private bool shouldSearch;


    
    private bool shouldAttack = false;
    private bool canAttack = false;

    private Animator anim;


    new void Start()
    {
        base.Start();
        anim = GetComponentInChildren<Animator>();
        health = maxHealth;
        shouldSearch = true;
    }


    private void Update()
    {

        if (!agent.isStopped)
        {
            if (agent.hasPath)
            {
                if (shouldAttack)
                {
                    if (Vector3.Distance(transform.position, targetWall.transform.position) < 2f)
                    {
                        StartAttack();
                    }
                }
            }
            else
            {
                UpdatePath();
            }

        }



        if (canAttack)
        {
            if (attackTarget != null)
            {
                if (attackTarget.isDead)
                {
                    EndAttack();
                }
                if (attackTime + timeLastAttack < Time.time)
                {
                    transform.LookAt(agent.destination);
                    anim.SetTrigger("Attack");
                    attackTarget.TakeDamage(attackDamage, null);
                    timeLastAttack = Time.time;
                }
            } else
            {
                EndAttack();
            }

        }
    }

    void UpdatePath()
    {
        lastSearchTime = Time.time;

        anim.SetBool("isWalking", true);
        //figure out if there is a path to player

        agent.CalculatePath(egg.transform.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            shouldAttack = false;
            agent.SetPath(path);
            return;
        }
        shouldAttack = true;
        targetWall = GetRandomWall();
        atttackTargetGO = targetWall.gameObject;
        attackTarget = targetWall;
        if (!agent.hasPath)
        {
            agent.destination = targetWall.transform.position;
        }


    }

    void StartAttack()
    {

        transform.LookAt(targetWall.transform.position);
        attackTarget = targetWall;
        anim.SetBool("isWalking", false);
        agent.isStopped = true;
        canAttack = true;
        print("start attack");
    }

    public void EndAttack()
    {

        attackTarget = null;
        shouldAttack = false;
        canAttack = false;
        agent.isStopped = false;
        UpdatePath();
    }

}

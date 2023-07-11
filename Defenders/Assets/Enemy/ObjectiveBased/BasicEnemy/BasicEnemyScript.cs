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

        if (canAttack)
        {
            if (attackTarget != null)
            {
                if (attackTarget.isDead)
                {
                    LeaveWall();
                }
                else
                {
                    if (attackTime + timeLastAttack < Time.time)
                    {
                        transform.LookAt(agent.destination);
                        anim.SetBool("isAttacking", true);
                        if (attackTarget != null)
                        {
                            attackTarget.GiveDamage(attackDamage);
                        }

                        timeLastAttack = Time.time;
                    }
                }

            }
            else
            {
                EndAttack();
            }

        }

        if (agent.isStopped)
        {
            return;
        }

        if (agent.hasPath)
        {
            return;
        }

        UpdatePath();


       
    }

    void UpdatePath()
    {
        lastSearchTime = Time.time;

        anim.SetBool("isWalking", true);
        //figure out if there is a path to player
        agent.destination = egg.transform.position;

    }

    void StartAttack()
    {

        transform.LookAt(targetWall.transform.position);
        attackTarget = targetWall;
        anim.SetBool("isWalking", false);
        agent.isStopped = true;
        canAttack = true;
    }

    public void EndAttack()
    {

        attackTarget = null;
        shouldAttack = false;
        canAttack = false;
        agent.isStopped = false;
        anim.SetBool("isAttacking", false);
        UpdatePath();
    }

    public override void AtWall(WallDefenceScript wall)
    {
        base.AtWall(wall);
        StartAttack();

    }
    public override void LeaveWall()
    {
        base.LeaveWall();
        EndAttack();
    } 

}

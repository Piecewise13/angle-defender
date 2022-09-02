using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyScript : ParentAIScript
{
    public float attackTime;
    private float timeLastAttack;
    private Damageable attackTarget;

    private Collider hitCollider;
    public LayerMask wall;

    [SerializeField] private float searchTime;
    private float lastSearchTime;
    private bool shouldSearch;


    
    private bool shouldAttack = false;

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
        if (shouldSearch)
        {
            if (lastSearchTime + searchTime < Time.time)
            {
                UpdatePath();

            }
        }
        if (shouldAttack)
        {
            if (attackTime + timeLastAttack < Time.time)
            {
                anim.SetTrigger("Attack");
                print(attackTarget);
                attackTarget.TakeDamage(attackDamage, null);
                timeLastAttack = Time.time;
            }
        }
    }

    void UpdatePath()
    {

        lastSearchTime = Time.time;
        agent.SetPath(path);
        anim.SetBool("isWalking", true);
        //figure out if there is a path to player

        agent.CalculatePath(egg.transform.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            return;
        }
        targetWall = GetRandomWall();
        agent.CalculatePath(targetWall.transform.position, path);

    }

    public void StartAttack(Damageable attackTarget)
    {
        shouldAttack = true;
        shouldSearch = false;
        this.attackTarget = attackTarget;
        anim.SetBool("isWalking", false);
    }

    public void EndAttack()
    {
        shouldAttack = false;
        shouldSearch = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyScript : ParentAIScript
{
    public float attackSpeed;
    private float timeLastAttack;
    public float attackDamage;

    private Damageable targetObject;
    private Collider hitCollider;
    public LayerMask wall;

    
    private bool shouldAttack;


    void Start()
    {
        initValues();

        findNewTarget();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(egg.transform, transform.up);

        if (shouldAttack)
        {
            if (timeLastAttack + attackSpeed < Time.time)
            {
                if (!targetObject.isDead)
                {
                    
                    targetObject.TakeDamage(attackDamage, hitCollider);
                    timeLastAttack = Time.time;
                    
                    timeLastAttack = Time.time;
                } else
                {
                    stopAttacking();
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.gameObject.tag.Equals("Enemy"))
        {
            startAttacking(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        stopAttacking();
    }

    public void findNewTarget()
    {
        
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(egg.transform.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.destination = egg.transform.position;
            
           
        }
        else
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, egg.transform.position, out hit, wall))
            {
                agent.destination = hit.transform.position;
                
            }
        }
    }


    private void startAttacking(Collider other)
    {
        try
        {
            targetObject = other.transform.root.gameObject.GetComponentInChildren<Damageable>();
            if (!targetObject.isDead)
            {
                timeLastAttack = Time.time;
                hitCollider = other;
                shouldAttack = true;
            }
            else
            {
                stopAttacking();
            }
        }
        catch
        {
            shouldAttack = false;
        }
    }

    private void stopAttacking()
    {
        
        shouldAttack = false;
        targetObject = null;
        hitCollider = null;
        findNewTarget();
    }



    public override void Death()
    {
        Destroy(gameObject.transform.root.gameObject);
    }

    public override void TakeDamage(float damage, Collider hitCollider)
    {
        health -= damage;
        
        if (health <= 0)
        {
            Death();
        }
    }

    public override void ReachedEgg()
    {
        
    }
}

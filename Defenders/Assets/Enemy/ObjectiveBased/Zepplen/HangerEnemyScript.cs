using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HangerEnemyScript : PlayerBasedAIParent
{


    Animator anim;
    Rigidbody rb;

    public float searchTime;
    private float lastSearchTime;


    public float attackTime;
    private float lastAttackTime;
    private bool shouldAttack = false;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (shouldAttack)
        {
            if (lastAttackTime + attackTime < Time.time)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }


        if (searchTime + lastSearchTime < Time.time)
        {

            UpdatePath();

            lastSearchTime = Time.time;
        }
    }


    void UpdatePath()
    {


        if (agent.isActiveAndEnabled)
        {

            player = GetClosestPlayer();
            if (player == null)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, egg.transform.position, out hit, LayerMask.NameToLayer("Defense")))
                {
                    agent.destination = hit.transform.position;

                } else
                {
                    agent.destination = egg.transform.position;
                }
                return;
            }

            NavMeshPath path = new NavMeshPath();
            //print(agent);
            agent.CalculatePath(player.transform.position, path);
            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                agent.destination = player.transform.position;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, egg.transform.position, out hit, LayerMask.NameToLayer("Defense")))
                {
                    agent.destination = hit.transform.position;

                }

            }
            anim.SetBool("isWalking", true);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            rb.isKinematic = true;
            agent.enabled = true;
            anim.SetTrigger("Landed");
        }
    }


    private void Attack()
    {
        print("Attacking the player");
        anim.SetTrigger("Attack");
        player.TakeDamage(attackDamage, null);
    }

    private void StopAttack()
    {

    }


    public override void PlayerFound(PlayerScript player)
    {
        this.player = player;
        shouldAttack = true;
    }

    public override void PlayerLost(PlayerScript player)
    {
        shouldAttack = false;
        UpdatePath();
    }
}

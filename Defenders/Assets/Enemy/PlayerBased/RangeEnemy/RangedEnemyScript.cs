using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyScript : PlayerBasedAIParent
{


    public float searchTime;
    private float lastSearchTime;

    [SerializeField] private float shootDistance;

    public float shootTime;
    private float lastShootTime;

    private GameObject shootingTarget;
    private bool isShooting;


    public GameObject fireball;
    public Transform fireballSpawn;

    private Animator anim;

    // Start is called before the first frame update
    new void Start()
    {

        base.Start();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (searchTime + lastSearchTime < Time.time)
        {
            if (!isShooting)
            {
                UpdatePath();
            }


            lastSearchTime = Time.time;
        }

        if (agent.hasPath)
        {
            if (agent.remainingDistance < shootDistance)
            {
                if (shootTime + lastShootTime < Time.time)
                {
                    if (!isShooting)
                    {
                        StartShooting();
                    }
                    agent.isStopped = true;
                    transform.LookAt(shootingTarget.transform.position);


                }
            }
        }
    }

    private void UpdatePath()
    {
        if (agent.isActiveAndEnabled)
        {

            player = ClosestPlayer();
            if (player == null)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, egg.transform.position, out hit, LayerMask.NameToLayer("Defense")))
                {
                    agent.destination = hit.transform.position;
                    shootingTarget = hit.transform.gameObject;
                }
                else
                {
                    agent.destination = egg.transform.position;
                    shootingTarget = egg;
                }
                return;
            }

            NavMeshPath path = new NavMeshPath();
            //print(agent);
            agent.CalculatePath(player.transform.position, path);
            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                agent.destination = player.transform.position;
                shootingTarget = player.gameObject;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, egg.transform.position, out hit, LayerMask.NameToLayer("Defense")))
                {
                    agent.destination = hit.transform.position;
                    shootingTarget = hit.transform.gameObject;
                }

            }
            anim.SetBool("isWalking", true);
        }

    }

    public void Shoot()
    {
        Instantiate(fireball, fireballSpawn.transform.position, fireballSpawn.transform.rotation).GetComponent<FireBall>().target = shootingTarget.transform;
        StopShooting();
        
    }

    private void StartShooting()
    {
        anim.SetTrigger("Fire");
        isShooting = true;


    }

    private void StopShooting()
    {
        lastShootTime = Time.time;
        lastSearchTime = Time.time;
        isShooting = false;
    }


    public override void PlayerFound(PlayerScript player)
    {



    }
    public override void PlayerLost(PlayerScript player)
    {

    }




}

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

    public GameObject shootingTarget;
    private bool isShooting;
    private bool shouldShoot;


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

        //REDESIGN THIS SO THAT IT ONLY UPDATES THE PATH WHEN NEEDED
        if (!agent.isStopped)
        {
            if (searchTime + lastSearchTime < Time.time)
            {
                player = GetClosestPlayer();
                if (player != null)
                {
                    agent.CalculatePath(player.transform.position, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        print("Going to player");
                        anim.SetBool("isWalking", true);
                        shouldShoot = true;
                        shootingTarget = player.gameObject;
                        agent.SetPath(path);
                        lastSearchTime = Time.time;
                        return;
                    }
                    UpdatePath();

                }
                else
                {
                    UpdatePath();
                }

            }
        }


        if (!agent.pathPending)
        {
            if (shouldShoot)
            {
                if (Vector3.Distance(transform.position, shootingTarget.transform.position) < shootDistance)
                {
                    transform.LookAt(shootingTarget.transform.position);
                    if (!isShooting)
                    {
                        Damageable script = shootingTarget.GetComponentInChildren<Damageable>();
                        if (script.isDead)
                        {
                            StopShooting();
                            return;
                        }
                        if (shootTime + lastShootTime < Time.time)
                        {
                            StartShooting();
                        }
                    }
                }
            }
        }


        /**
         * 
         * 
         *
        if (!isShooting)
        {



            if (searchTime + lastSearchTime < Time.time)
            {
                UpdatePath();


            }

        }


        //agent.remaining distance is 0 even when it  has path
        if (!agent.pathPending)
        {
            if (shouldShoot)
            {
                if (agent.remainingDistance < shootDistance)
                {
                   
                    if (shootTime + lastShootTime < Time.time)
                    {
                        if (shootingTarget)
                        {

                        }



                        if (!isShooting)
                        {
                            StartShooting();
                        }
                        transform.LookAt(shootingTarget.transform.position);


                    }
                }
            }
        }
        *
        **/
        

    }

    private void UpdatePath()
    {


        /*
         * 
         * THIS WORKS BUT ALSO MIGHT NOT BE VERY EFFICENT. TEST WITH SHITTY COMPUTERS TO MAKE SURE ITS THE BEST WAY
         */

        lastSearchTime = Time.time;
        anim.SetBool("isWalking", true);
        agent.CalculatePath(egg.transform.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetPath(path);
            shootingTarget = null;
            shouldShoot = false;
            return;
        }
        targetWall = GetRandomWall();
        shootingTarget = targetWall.gameObject;
        agent.destination = targetWall.transform.position;
        shouldShoot = true;
        /**
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
                shootingTarget = player.gameObject;
                shouldShoot = true;
                return;
            }
        }

        agent.CalculatePath(egg.transform.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetPath(path);
            shootingTarget = null;
            shouldShoot = false;
            return;
        }
        targetWall = GetRandomWall();
        shootingTarget = targetWall.gameObject;
        agent.destination = targetWall.transform.position;
        shouldShoot = true;
        */
    }


    public void Shoot()
    {
        Instantiate(fireball, fireballSpawn.transform.position, fireballSpawn.transform.rotation).GetComponent<FireBall>().target = shootingTarget.transform;
        StopShooting();
        
    }

    private void StartShooting()
    {
        anim.SetTrigger("Fire");
        anim.SetBool("isWalking", false);
        agent.isStopped = true;
        isShooting = true;


    }

    private void StopShooting()
    {
        agent.isStopped = false;
        lastShootTime = Time.time;
        isShooting = false;
    }


    public override void PlayerFound(PlayerScript player)
    {



    }
    public override void PlayerLost(PlayerScript player)
    {

    }




}

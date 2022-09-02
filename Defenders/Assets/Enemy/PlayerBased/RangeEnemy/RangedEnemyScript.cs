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

        if (!isShooting)
        {
            if (searchTime + lastSearchTime < Time.time)
            {

                UpdatePath();
            }

            if (agent.hasPath)
            {
                if (shouldShoot)
                {
                    if (agent.remainingDistance < shootDistance)
                    {
                        if (shootTime + lastShootTime < Time.time)
                        {
                            if (!isShooting)
                            {
                                StartShooting();
                            }
                            transform.LookAt(shootingTarget.transform.position);


                        }
                    }
                }

            }
        }

    }

    private void UpdatePath()
    {


        /*
         * 
         * THIS WORKS BUT ALSO MIGHT NOT BE VERY EFFICENT. TEST WITH SHITTY COMPUTERS TO MAKE SURE ITS THE BEST WAY
         */
        lastSearchTime = Time.time;
        agent.SetPath(path);
        anim.SetBool("isWalking", true);
        //figure out if there is a path to player
        player = GetClosestPlayer();
        if (player != null)
        {
            agent.CalculatePath(player.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {

                shootingTarget = player.gameObject;
                shouldShoot = true;
                return;
            }
        }

        agent.CalculatePath(egg.transform.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            shouldShoot = false;
            return;
        }
        targetWall = GetRandomWall();
        agent.CalculatePath(targetWall.transform.position, path);
        shouldShoot = true;
    }

    public void Shoot()
    {
        Instantiate(fireball, fireballSpawn.transform.position, fireballSpawn.transform.rotation).GetComponent<FireBall>().target = shootingTarget.transform;
        StopShooting();
        
    }

    private void StartShooting()
    {
        anim.SetTrigger("Fire");
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

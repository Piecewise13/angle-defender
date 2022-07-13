using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyScript : PlayerBasedAIParent
{



    private bool isShooting;
    [SerializeField] private float shootDelay;
    private float lastShootTime;
    private bool isShootingWall;
    public LayerMask wall;

    private Damageable shootingTarget;
    WallDefenceScript targetWall;

    // Start is called before the first frame update
    void Start()
    {
        
        initValues();
        InvokeRepeating("UpdatePath", 2f, 2f);
        targetWall = GetClosestWall(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            if (player != null)
            {


                transform.LookAt(player.transform, transform.up);
                if (lastShootTime + shootDelay < Time.time)
                {
                    lastShootTime = Time.time;
                    Shoot();

                }
            } else
            {
                StopShooting();
            }
        }
    }

    private void UpdatePath()
    {
        player = ClosestPlayer(transform.position);
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(player.transform.position, path);
        if(path.status == NavMeshPathStatus.PathComplete)
        {
            if (isShootingWall)
            {
                StopShooting();
            }
            agent.destination = player.transform.position;
        }
        else
        {
            
            agent.destination = targetWall.transform.position;

            if (agent.remainingDistance < playerRange)
            {
                StartShooting((Damageable)targetWall);
                isShootingWall = true;
            }
        }

        //if (Vector3.Distance(player.transform.position, transform.position) < playerRange)
        //{
        //    StartShooting();

        //}
        //else
        //{
        //    if (isShooting)
        //    {
        //        StopShooting();
        //    }
        //}
    }

    private void Shoot()
    {
        shootingTarget.TakeDamage(attackDamage, null);
    }

    private void StartShooting(Damageable target)
    {
        isShooting = true;
        agent.isStopped = true;
        shootingTarget = target;

    }

    private void StopShooting()
    {
        isShootingWall = false;
        agent.isStopped = false;
        isShooting = false;
        targetWall = GetClosestWall(transform.position);
    }


    public override void Death()
    {
        Destroy(gameObject);
    }

    public override void PlayerFound(PlayerScript player)
    {
        StartShooting((Damageable)player);
    }
    public override void PlayerLost(PlayerScript player)
    {
        StopShooting();
    }

    public override void ReachedEgg()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float damage, Collider hitCollider)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }


}

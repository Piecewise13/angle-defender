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
    public LayerMask notPlayer;

    private Damageable shootingTarget;
    private GameObject shootingTargetGO;
    WallDefenceScript targetWall;

    // Start is called before the first frame update
    new void Start()
    {

        base.Start();
        InvokeRepeating("UpdatePath", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            if (shootingTarget != null)
            {

                transform.LookAt(shootingTargetGO.transform, Vector3.up);
                if (lastShootTime + shootDelay < Time.time)
                {

                    if (Physics.Linecast(transform.position, shootingTargetGO.transform.position, notPlayer) && !isShootingWall)
                    {
                        print("i cant see him");
                        UpdatePath();
                        lastShootTime = Time.time;
                        return;
                    }
                    

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
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(egg.transform.position, path);
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            targetWall = GetClosestWall(transform.position);
            print(targetWall);

            agent.destination = targetWall.transform.position;

            if (agent.remainingDistance < 20f && agent.hasPath)
            {

                StartShooting((Damageable)targetWall);
                shootingTargetGO = targetWall.gameObject;
                isShootingWall = true;
            }
        }
        else
        {
            StopShooting();
            agent.destination = egg.transform.position;
            return;
        }

        player = ClosestPlayer(transform.position);

        if (player != null)
        {
            
            agent.CalculatePath(player.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                if (isShootingWall)
                {
                    StopShooting();
                }
                agent.destination = player.transform.position;
                print("Moving to player");
                if (Physics.CheckSphere(transform.position, 20f, player.gameObject.layer))
                {
                    StartShooting((Damageable)player);
                    shootingTargetGO = player.gameObject;
                    
                }
                return;
            }
        }


            
    }

    private void Shoot()
    {
        if (shootingTarget.isDead)
        {
            shootingTarget = null;
            shootingTargetGO = null;
            return;
        }
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
        shootingTarget = null;
        shootingTargetGO = null;
        isShootingWall = false;
        agent.isStopped = false;
        isShooting = false;
    }


    public override void Death()
    {
        Destroy(gameObject);
    }

    public override void PlayerFound(PlayerScript player)
    {

            StartShooting((Damageable)player);
            shootingTargetGO = player.gameObject;


    }
    public override void PlayerLost(PlayerScript player)
    {
        StopShooting();
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

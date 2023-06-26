using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Ranged_Script : PlayerBasedAIParent
{
    private bool isFiring;

    private float lastFireTime;
    [SerializeField] private float fireRate;

    [SerializeField] private float playerSearchTime;
    private float lastSearchTime;

    public new void Start()
    {
        base.Start();
        GoToEgg();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPlayer)
        {
            if (lastFireTime + fireRate < Time.time)
            {
                Shoot();
                lastFireTime = Time.time;
            }
            Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(target);
        }

        if (playerSearchTime + lastSearchTime < Time.time)
        {
            SearchForPlayer();
            lastSearchTime = Time.time;
        }

    }

    public override void PlayerFound(PlayerScript player)
    {
        hasPlayer = true;
        agent.isStopped = true;
        base.player = player;
        lastFireTime = Time.time;
        
    }

    public override void PlayerLost(PlayerScript player)
    {
        agent.isStopped = false;
        hasPlayer = false;
        GoToEgg();
        base.player = null;
    }

    private void Shoot()
    {
        //TRICKY PART WILL BE ANIMATING THE ENEMY TO ROTATE AND FACE THE ENEMY
        player.GiveDamage(attackDamage);

    }

    private void SearchForPlayer()
    {

        if (Physics.CheckSphere(transform.position, playerSearchRange, LayerMask.GetMask("Player")))
        {
            print("Player Found");
            PlayerFound(GetClosestPlayer());
            return;
        }

        if (hasPlayer)
        {
            print("Player Lost");
            PlayerLost(player);
        }
    }
}

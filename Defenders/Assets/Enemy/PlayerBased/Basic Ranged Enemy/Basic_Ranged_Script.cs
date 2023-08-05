using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Ranged_Script : PlayerBasedAIParent
{
    private bool isFiring;

    private float lastFireTime;
    [SerializeField] private float fireRate;

    [SerializeField] private float targetSearchTime;
    private float lastSearchTime;

    public LayerMask shotBlockingLayers;

    public Animator anim;

    private Damageable target;
    public GameObject targetObj;

    public new void Start()
    {
        base.Start();
        GoToEgg();
        anim.SetBool("isWalking", true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            Vector3 targetPos = new Vector3(targetObj.transform.position.x, transform.position.y, targetObj.transform.position.z);
            transform.LookAt(targetPos);
        }

        if (targetSearchTime + lastSearchTime < Time.time)
        {
            SearchForTarget();
        }

    }

    public void FoundTarget(GameObject targetObj)
    {
        hasTarget = true;
        target = targetObj.GetComponentInChildren<Damageable>();
        this.targetObj = targetObj;
        agent.isStopped = true;
        anim.SetBool("isFiring", true);
        anim.SetBool("isWalking", false);

    }

    public void LostTarget()
    {
        if (!hasTarget)
        {
            return;
        }

        hasTarget = false;
        agent.isStopped = false;
        GoToEgg();
        target = null;
        targetObj = null;
        anim.SetBool("isFiring", false);
        anim.SetBool("isWalking", true);
    }

    public void Shoot()
    {
        //TRICKY PART WILL BE ANIMATING THE ENEMY TO ROTATE AND FACE THE ENEMY
        target.GiveDamage(attackDamage);
        SearchForTarget();

    }

    private bool HasLineOfSight(GameObject other)
    {
        return !Physics.Linecast(transform.position + Vector3.up * 2, other.transform.position + Vector3.up * 2, shotBlockingLayers);
    }

    private void SearchForTarget()
    {
        lastSearchTime = Time.time;

        if (Physics.CheckSphere(transform.position, targetSearchRange, LayerMask.GetMask("Player")))
        {

            PlayerScript player = GetClosestPlayer();
            if(HasLineOfSight(player.gameObject))
            {
                FoundTarget(player.gameObject);
                return;
            }
        }

        RaycastHit hit;


        if (Physics.Raycast(transform.position + Vector3.up * 2, egg.transform.position - transform.position, out hit, targetSearchRange, LayerMask.GetMask("Defense")))
        {
            FoundTarget(hit.collider.transform.root.gameObject);
            return;
        }

        LostTarget();
    }

    public override void PlayerFound(PlayerScript player)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerLost(PlayerScript player)
    {
        throw new System.NotImplementedException();
    }
}

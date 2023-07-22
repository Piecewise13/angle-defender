using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlamScript : ParentUltimateAbility
{

    private float slamSpeed = 30f;

    private float maxDistance = 40f;

    private float maxRadius = 15f;

    private float maxDamage = 100f;

    private bool isSlamming = false;

    private float distanceFromGround;

    private Rigidbody rb;
    

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = player.GetRigidbody();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSlamming)
        {
            return;
        }

        if (player.isGrounded)
        {
            DamageEnemies();
            return;
        }

        rb.AddForce(Vector3.down * slamSpeed, ForceMode.Force);
    }


    private void DamageEnemies()
    {
        float multiplier = distanceFromGround / maxDistance;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, maxRadius * multiplier, LayerMask.GetMask("EntityTrigger"));


        foreach (var item in hitEnemies)
        {
            if (item.tag != "Enemy")
            {
                continue;
            }

            Damageable aiScript = GetComponent<Damageable>();

            aiScript.GiveDamage(maxDamage * multiplier);
        }
        EndAbility();
    }

    public override void ActivateAbility()
    {
        if (isSlamming)
        {
            return;
        }
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground", "Block", "Default")))
        {
            distanceFromGround = hit.distance;
            print(hit.distance);
        }
        isSlamming = true;
        player.SetCanControlPlayer(false);
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.down * slamSpeed, ForceMode.Impulse);
    }

    public override void EndAbility()
    {
        isSlamming = false;
        distanceFromGround = 0f;
        player.SetCanControlPlayer(true);
    }
}

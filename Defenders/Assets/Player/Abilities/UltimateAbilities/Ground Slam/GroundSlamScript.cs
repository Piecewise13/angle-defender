using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlamScript : ParentUltimateAbility
{

    private float slamSpeed = 30f;

    private float maxDistance = 100f;

    private float maxRadius = 20f;

    private float maxDamage = 400f;

    private bool isSlamming = false;

    private float distanceFromGround;

    private Rigidbody rb;

    private GameObject particle;
    

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = player.GetRigidbody();
        particle = Resources.Load("GroundSlamParticles") as GameObject;
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
        print(distanceFromGround);
        float multiplier = Mathf.Clamp(distanceFromGround / maxDistance, 0.3f, 1f);

        print(multiplier);

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, maxRadius * multiplier, LayerMask.GetMask("Enemy"));


        foreach (var item in hitEnemies)
        {

            Damageable aiScript = item.transform.root.GetComponentInChildren<Damageable>();
            print("Gave " + item.transform.root.gameObject + " " + maxDamage * multiplier + " damage");


            aiScript.GiveDamage(maxDamage * multiplier);
        }

        ParticleSystem groundSlam = Instantiate(particle, transform.position, Quaternion.Euler(-90f, 0f, 0f)).GetComponent<ParticleSystem>();

        var hemi = groundSlam.shape;
        hemi.shapeType = ParticleSystemShapeType.Cone;
        hemi.radius = maxRadius * multiplier;

        var main = groundSlam.main;
        main.startSizeMultiplier = 500 * multiplier;
        main.startSpeed = 8 * multiplier;

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

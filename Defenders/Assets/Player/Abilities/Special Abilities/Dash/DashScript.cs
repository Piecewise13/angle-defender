using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : ParentSpecialAbility
{

    private float dashForce = 50f;
    private float dashMaxSpeed = 60f;

    private float previousMovementSpeed;

    private float dashingTime = .6f;
    private float startDashingTime;
    private bool isDashing = false;

    private Rigidbody rb;

    public new void Start()
    {
        base.Start();
        rb = player.GetRigidbody();

        
    }

    
    public override void ActivateAbility()
    {
        print("Using Ability");
        previousMovementSpeed = player.GetMaxSpeed();
        Rigidbody rb = player.GetRigidbody();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.AddForce(player.transform.forward * dashForce, ForceMode.Impulse);
        previousMovementSpeed = player.GetMaxSpeed();
        player.SetMaxSpeed(dashMaxSpeed);
        isDashing = true;
        startDashingTime = Time.time;
    }

    public override void EndAbility()
    {
        rb.useGravity = true;
        rb.velocity /= 2f;
        player.SetMaxSpeed(previousMovementSpeed);
        isDashing = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            return;
        }

        if (dashingTime + startDashingTime < Time.time)
        {
            EndAbility();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBased : PlayerScript
{



    //GROUND SLAM STUFF
    [Space(20)]
    [Header("Ground Slam")]

    [SerializeField] private float slamBaseRadius;
    [SerializeField] private float groundSlamDamage;
    private bool canUlt;
    private bool isGroundSlam;
    private float groundSlamSpeed;
    private float groundSlamTimer;
    public LayerMask ultLayerMask;
    private float ultGroundDist;



    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {

        /*
         * MOVEMENT REGION
       */
        #region

      isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canJump = true;
            canWallKick = true;
            animator.SetBool("isFalling", false);
        }

        if (velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("isFalling", true);
        }








        if (canMove)
        {
            forwardValue = Input.GetAxis("Vertical");
            sideValue = Input.GetAxis("Horizontal");


            moveDir = forwardValue * transform.forward + sideValue * transform.right;
            if (!moveDir.Equals(Vector3.zero))
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);

            }

            if (firstPerkUnlocked)
            {
                if (!isGrounded && canWallKick)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        print("wall jump");
                        RaycastHit hit;

                        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, wallKickDistance, movementLayer))
                        {
                            velocity.y = wallKickHeight;
                            canWallKick = false;
                        }
                    }
                }

                if (Input.GetButtonDown("Dash"))
                {
                    if (!isDashing && soulFire >= dashCost)
                    {
                        print("dashing");
                        isDashing = true;
                        SetSoulFire(-dashCost);
                        startDashTime = Time.time;
                        initForward = transform.forward;
                    }
                }
            }


            if (thirdPerkUnlocked)
            {
                if (Input.GetButtonDown("Ult"))
                { 

                    if (!isGrounded && canUlt)
                    {
               
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, Vector3.down,  out hit, Mathf.Infinity, ultLayerMask))
                        {
                            ultGroundDist = hit.distance;
                            isGroundSlam = true;
                        
                        }
                    }
                }
            }



            if (Input.GetButtonDown("Jump") && canJump)
            {
                if (shouldADH)
                {


                    movementSpeedVar = (movementSpeedVar * 1.1f);
                    abhCount++;


                }
                else
                {
                    movementSpeedVar = defaultMovementSpeed;
                }
                tickCounter = 0;
                canJump = false;
                velocity.y = jumpHeight;

                animator.SetTrigger("isJumping");
                animator.SetBool("isFalling", true);
            }

            if (isDashing)
            {
                if (dashTime + startDashTime > Time.time)
                {
                    print("should be actually dashing");
                    controller.Move(initForward * dashSpeed * Time.deltaTime);
                }
                else
                {
                    isDashing = false;
                }

            }
            else if (isGroundSlam)
            {
                if (isGrounded)
                {
                    GroundSlam();
                    isGroundSlam = false;
                    groundSlamSpeed = 10f;
                    groundSlamTimer = 0f;
                    
                } else
                {
                    controller.Move(Vector3.down * groundSlamSpeed * Time.deltaTime);
                    groundSlamSpeed += Mathf.Pow(groundSlamTimer, 2);
                    groundSlamTimer += Time.deltaTime;
                }
 
            } else
            {
                controller.Move(moveDir * movementSpeedVar * Time.deltaTime);
            }
        }




        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        #endregion


        if (isDead)
        {
            if (deathTime + respawnTime < Time.time)
            {
                Respawn();
            }
        }

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);

    }

    private void GroundSlam() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, slamBaseRadius * (ultGroundDist / 10f), LayerMask.NameToLayer("Enemy"));
        foreach (Collider item in colliders)
        {
            Damageable script = item.GetComponentInParent<Damageable>();
            script.TakeDamage(groundSlamDamage, item);
        }
    }
}

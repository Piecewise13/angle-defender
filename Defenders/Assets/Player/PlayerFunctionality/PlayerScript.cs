using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerScript : MonoBehaviour, Damageable
{

    /*
     * PLAYER COMPONENTS VARS
     */
    [Header("Component Vars")]
    public CharacterController controller;
    public Camera playerCamera;
    [SerializeField] private Transform groundCheck;
    [HideInInspector]public MouseLook lookScript;
    private WeaponInventoryManager weaponManager;
    public Animator animator;
    public GameObject deathScreen;

    [Space(20)]
    [Header("Weapon Vars")]
    /*
     * WEAPON VARS
     */
    private float defaultFov = 60f;
    private float zoomSpeed = 10f;
    private float targetFOV = 60f;
    [SerializeField] private int soulFire;
    [SerializeField] private int maxSoulFire;

    /*
     * HUD SCRIPTS
     */
    [Space(20)]
    [Header("HUD Vars")]
    public HUDScript hudScript;
    [SerializeField] private UpgradeTreeScript upgradeTree;

    /**
     * MOVEMENT VARS
     */
    [Space(20)]
    [Header("Movement Vars")]
    #region Movment Vars
    private bool canMove = true;
    [SerializeField] private float defaultMovementSpeed;
    private float movementSpeedVar = 10f;
    [SerializeField] private float jumpHeight;
    private float forwardValue;
    private float sideValue;
    private bool canJump;

    private Vector3 moveDir;
    private float  gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Space(10)]
    //Wall Kick Vars
    [SerializeField] private float wallKickDistance;
    [SerializeField] private float wallKickHeight;
    private bool canWallKick = true;
    private bool movementUnlocked = true;
    public LayerMask movementLayer;


    [Space(10)]
    //Dash Vars
    [SerializeField] private float dashTime;
    private float startDashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private int dashCost;
    public GameObject dashTrigger;
    private bool canDash = true;
    private bool isDashing;
    private Vector3 initForward;


    [Space(10)]
    //ABH
    [SerializeField] private int tickMax;
    [SerializeField] private float abhSpeed;
    [SerializeField] private int abhCount;
    private bool canABH;
    private int tickCounter;
    private bool shouldADH;




    #endregion

    /**
     * RESOURCE VAR
     */
    [Space(20)]
    [Header("Resource Vars")]
    #region Resources Vars


    [SerializeField] private int woodAmount;
    [SerializeField] private int ironAmount;
    [SerializeField] private int diamondAmount;

    #endregion



    public float health { get; set; }
    public bool isDead { get; set; }

    [Space(20)]
    [Header("Player Vars")]
    public float maxHealth;



    private float respawnTime = 5f;
    private float deathCount;
    [SerializeField] private float respawnSlope;
    private float deathTime;
    public Transform spawnPoint;



    // Start is called before the first frame update
    void Start()
    {

        lookScript = GetComponentInChildren<MouseLook>();
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        weaponManager = GetComponent<WeaponInventoryManager>();
        animator = GetComponent<Animator>();

        upgradeTree.gameObject.SetActive(false);
        defaultFov = playerCamera.fieldOfView;
        movementSpeedVar = defaultMovementSpeed;
        dashTrigger.SetActive(false);
        health = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        /**
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
            } else
            {
                animator.SetBool("isWalking", false);
                
            }

            if (movementUnlocked)
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
                        dashTrigger.SetActive(true);
                        isDashing = true;
                        SetSoulFire(-dashCost);
                        startDashTime = Time.time;
                        initForward = transform.forward;
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
                if (dashTime + startDashTime >  Time.time)
                {
                    print("should be actually dashing");
                    controller.Move(initForward * dashSpeed * Time.deltaTime);
                } else
                {
                    dashTrigger.SetActive(false);
                    isDashing = false;
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

    public void FixedUpdate()
    {
        if (forwardValue == -1f)
        {
            if (!isGrounded && !canJump )
            {
                canABH = true;
                tickCounter = 0;
            } else


            if (canABH)
            {
                if (isGrounded)
                {
                    if (tickCounter > tickMax)
                    {
                        print("lost abh");
                        movementSpeedVar = defaultMovementSpeed;
                        shouldADH = false;
                        canABH = false;
                        tickCounter = 0;
                        abhCount = 1;
                    } else
                    {

                        shouldADH = true;
                        tickCounter++;
                    }
                }

            }

        } else
        {
            shouldADH = false;
            movementSpeedVar = defaultMovementSpeed;
        }
    }




    public void ChangeCameraZoom(float amount)
    {
        targetFOV = defaultFov / amount;
    }





    //public void upgradeTreeOpen(bool isOpen)
    //{


    //    if (isOpen)
    //    {
    //        hudScript.gameObject.SetActive(false);
    //        upgradeTree.gameObject.SetActive(true);
    //        Cursor.lockState = CursorLockMode.Confined;
    //        lookScript.setCanLook(false);
    //        canMove = false;
    //        weaponManager.canShoot(false);
    //    }
    //    else
    //    {
    //        hudScript.gameObject.SetActive(true);
    //        upgradeTree.gameObject.SetActive(false);
    //        Cursor.lockState = CursorLockMode.Locked;
    //        lookScript.setCanLook(true);
    //        canMove = true;
    //        weaponManager.canShoot(true);
    //    }
    //}

    public void openUIElement(bool isOpen)
    {
        if (isOpen)
        {
            SetAllBoolsFalse();
            ChangeCameraZoom(1f);
            hudScript.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            lookScript.setCanLook(false);
            canMove = false;
            weaponManager.canShoot(false);

        }
        else
        {
            hudScript.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            lookScript.setCanLook(true);
            canMove = true;

            weaponManager.canShoot(true);
        }
    }



    public void ChangeWeaponAnimationOverride(AnimatorOverrideController overrideController)
    {
        animator.runtimeAnimatorController = overrideController;
    }

    public void TakeDamage(float damage, Collider hitCollider)
    {
        if (!isDead)
        {
            health -= damage;
            hudScript.UpdateHealth();
            lookScript.ShakeCamera(damage);
            if (health <= 0)
            {
                Death();
            }
        }
    }

    public void Death() 
    {
        if (isDead)
        {
            return;
        }
        //play death animation
        deathTime = Time.time;

        respawnTime = respawnSlope * deathCount + 5f;
        deathCount++;
        controller.enabled = false;
        ChangeCameraZoom(1f);
        SetAllBoolsFalse();
        isDead = true;
        canMove = false;
        canJump = false;
        soulFire = 0;
        deathScreen.SetActive(true);
    }

    public void Respawn()
    {
        print("Respawning");
        //play respawn anim probably a courotine
        deathScreen.SetActive(false);
        transform.position = spawnPoint.position;
        controller.enabled = true;
        health = maxHealth;
        isDead = false;
        canJump = true;
        canMove = true;
        hudScript.UpdateHealth();
        hudScript.UpdateSoulFireValues();

    }

    public void UnlockMovement()
    {
        movementUnlocked = true;
    }


    /*
     * GET AND SET VARS
     */
    #region Get Set Functions

    public void SetResourceAmount(ResourceType type, int delta)
    {
        switch (type)
        {
            case ResourceType.Wood:
                woodAmount += delta;
                break;
            case ResourceType.Iron:
                ironAmount += delta;
                break;

            case ResourceType.Diamond:
                diamondAmount += delta;
                break;

        }
        if (delta == 0)
        {
            return;
        }
        hudScript.UpdateResourceValues();
        hudScript.ResoucesChangeFade(type, delta);
    }

    public int GetResourceAmount(ResourceType type)
    {

        switch (type)
        {
            case ResourceType.Wood:
                return woodAmount;

            case ResourceType.Iron:
                return ironAmount;

            case ResourceType.Diamond:
                return diamondAmount;

        }
        return -1;
    }

    public int GetSoulFire()
    {
        return soulFire;
    }

    public void SetSoulFire(int delta)
    {
        soulFire += delta;
        hudScript.UpdateSoulFireValues();
    }


    public void SetSoulFireMax(int delta)
    {
        maxSoulFire += delta;
        hudScript.UpdateSoulFireValues(); 
    }

    public int GetSoulFireMax()
    {
        return maxSoulFire;
    }

    public float GetRespawnTime()
    {
        return respawnTime;
    }

    private void SetAllBoolsFalse()
    {
        foreach (var item in animator.parameters)
        {
            if(item.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(item.name, false);
            }
        }
    }

    #endregion
}

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
    [SerializeField] protected Transform groundCheck;
    [HideInInspector] public MouseLook lookScript;
    protected WeaponInventoryManager weaponManager;
    public Animator animator;
    public GameObject deathScreen;

    [Space(20)]
    [Header("Weapon Vars")]
    /*
     * WEAPON VARS
     */
    protected float defaultFov = 60f;
    protected float zoomSpeed = 10f;
    protected float targetFOV = 60f;
    [SerializeField] protected int soulFire;
    [SerializeField] protected int maxSoulFire;

    /*
     * HUD SCRIPTS
     */
    [Space(20)]
    [Header("HUD Vars")]
    public HUDScript hudScript;
    [SerializeField] protected UpgradeTreeScript upgradeTree;
    [SerializeField] protected GameObject settingsMenu;
    protected bool inSettings;


    /**
     * MOVEMENT VARS
     */
    [Space(20)]
    [Header("Movement Vars")]
    #region Movment Vars
    protected bool canMove = true;
    [SerializeField] protected float defaultMovementSpeed;
    protected float movementSpeedVar = 10f;
    [SerializeField] protected float jumpHeight;
    protected float forwardValue;
    protected float sideValue;
    protected bool canJump;

    protected Vector3 moveDir;
    protected float gravity = -9.81f;
    protected Vector3 velocity;
    protected bool isGrounded;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    [Space(10)]
    [Header("Sonic Attack")]
    public float sonicEffectTime;
    public float sonicMovementSpeed;
    private bool isSonicEffect;
    private float startSonicEffectTime;


    [Space(10)]
    [Header("Wall Kick")]
    //Wall Kick Vars
    [SerializeField] protected float wallKickDistance;
    [SerializeField] protected float wallKickHeight;
    protected bool canWallKick = true;
    public LayerMask movementLayer;


    [Space(10)]
    [Header("Dash Vars")]
    //Dash Vars
    [SerializeField] protected float dashTime;
    protected float startDashTime;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected int dashCost;
    protected bool canDash = true;
    protected bool isDashing;
    protected Vector3 initForward;
    public GameObject dashTrigger;


    [Space(10)]
    [Header("ABH Vars")]
    //ABH
    [SerializeField] protected int tickMax;
    [SerializeField] protected int abhCount;
    protected bool canABH;
    protected int tickCounter;
    protected bool shouldADH;

    protected bool firstPerkUnlocked = false;
    protected bool secondPerkUnlocked = false;
    protected bool thirdPerkUnlocked = false;

    [Space(10)]
    [Header("Ladder Vars")]
    public float ladderSpeed;
    private bool isOnLadder;





    #endregion

    /**
     * RESOURCE VAR
     */
    [Space(20)]
    [Header("Resource Vars")]
    #region Resources Vars


    [SerializeField] protected int woodAmount;
    [SerializeField] protected int ironAmount;
    [SerializeField] protected int diamondAmount;

    #endregion



    public float health { get; set; }
    public bool isDead { get; set; }

    [Space(20)]
    [Header("Player Vars")]
    public float maxHealth;

    [Space(20)]
    [Header("Death Vars")]
    [SerializeField] protected Camera deathCam;
    protected float respawnTime = 5f;
    protected float deathCount;
    [SerializeField] protected float respawnSlope;
    protected float deathTime;
    public Transform spawnPoint;



    // Start is called before the first frame update
    protected void Start()
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
        hudScript.UpdateHealth();

    }

    // Update is called once per frame
    protected void Update()
    {
        /**
         * MOVEMENT REGION
         */
        #region

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            if (isOnLadder)
            {
                velocity.y = 0;
            }
            else
            {
                velocity.y = -2f;
            }
            canJump = true;
            canWallKick = true;

        }






        if (isSonicEffect)
        {
            if (sonicEffectTime + startSonicEffectTime < Time.time)
            {
                isSonicEffect = false;
                movementSpeedVar = defaultMovementSpeed;
            }
            movementSpeedVar = sonicMovementSpeed;
        }



        if (canMove)
        {
            forwardValue = Input.GetAxis("Vertical");
            sideValue = Input.GetAxis("Horizontal");


            if (!isOnLadder)
            {
                moveDir = forwardValue * transform.forward + sideValue * transform.right;
                movementSpeedVar = defaultMovementSpeed;
            }
            else
            {
                moveDir = forwardValue * Vector3.up + sideValue * transform.right;
                movementSpeedVar = ladderSpeed;
            }

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


                    movementSpeedVar = (movementSpeedVar * 1.2f);
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
                    dashTrigger.SetActive(false);
                    isDashing = false;
                }

            }
            else
            {
                controller.Move(moveDir * movementSpeedVar * Time.deltaTime);
            }
        }

        if (!isOnLadder)
        {
            velocity.y += gravity * Time.deltaTime;

        }

        controller.Move(velocity * Time.deltaTime);
        #endregion


        if (isDead)
        {
            if (deathTime + respawnTime < Time.time)
            {
                Respawn();
            }
        }

        if (Input.GetButtonDown("Settings"))
        {
            inSettings = !inSettings;
            openUIElement(inSettings);
            settingsMenu.SetActive(inSettings);
        }

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    public void FixedUpdate()
    {
        if (forwardValue == -1f)
        {
            if (!isGrounded && !canJump)
            {
                canABH = true;
                tickCounter = 0;
            }
            else


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
                    }
                    else
                    {

                        shouldADH = true;
                        tickCounter++;
                    }
                }

            }

        }
        else
        {
            shouldADH = false;
            movementSpeedVar = defaultMovementSpeed;
        }
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
        animator.SetBool("isDead", true);
        //deathCam.enabled = true;
        //playerCamera.enabled = false;

        deathCam.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        deathScreen.SetActive(true);
    }

    public void Respawn()
    {
        animator.SetBool("isDead", false);
        deathCam.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        //playerCamera.enabled = true;
        //deathCam.enabled = false;
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

    public virtual void UnlockFirstUpgrade()
    {
        firstPerkUnlocked = true;
    }


    public virtual void UnlockSecondUpgrade()
    {

    }

    public virtual void UnlockUlt()
    {

    }

    public void SonicAttackEffect()
    {
        print("hit be effect");
        isSonicEffect = true;
        startSonicEffectTime = Time.time;
    }


    public void ChangeWeaponAnimationOverride(AnimatorOverrideController overrideController)
    {
        animator.runtimeAnimatorController = overrideController;
    }

    public void ChangeCameraZoom(float amount)
    {
        targetFOV = defaultFov / amount;
    }

    public bool CanAffordResources(int woodCost, int ironCost, int diamondCost)
    {
        if (woodAmount >= woodCost &&
    ironAmount >= ironCost &&
    diamondAmount >= diamondCost)
        {
            return true;
        }

        return false;
    }


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

    public void SetResourceAmount(int woodDelta, int ironDelta, int diamondDelta)
    {
        woodAmount += woodDelta;
        ironAmount += ironDelta;
        diamondAmount += diamondDelta;
        hudScript.UpdateResourceValues();
        if (woodDelta != 0)
        {
            hudScript.ResoucesChangeFade(ResourceType.Wood, woodDelta);
        }
        if (ironDelta != 0)
        {
            hudScript.ResoucesChangeFade(ResourceType.Iron, ironDelta);
        }
        if (diamondDelta != 0)
        {
            hudScript.ResoucesChangeFade(ResourceType.Diamond, diamondDelta);
        }
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

    public void LaunchPlayer(float height)
    {
        velocity.y = height;
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

    public void SetHealthMax(int delta)
    {
        maxHealth += delta;
    }

    public float GetRespawnTime()
    {
        return respawnTime;
    }

    protected void SetAllBoolsFalse()
    {
        foreach (var item in animator.parameters)
        {
            if (item.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(item.name, false);
            }
        }
    }

    public void SetIsOnLadder(bool value)
    {
        isOnLadder = value;
    }

    public bool GetIsOnLadder()
    {
        return isOnLadder;
    }

    #endregion
}

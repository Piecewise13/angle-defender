using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class PlayerScript : MonoBehaviour, Damageable
{

    /*
     * PLAYER COMPONENTS VARS
     */
    [Header("Component Vars")]
    [HideInInspector]public Camera playerCamera;
    [SerializeField] protected Transform groundCheck;
    [HideInInspector] public MouseLook lookScript;
    [HideInInspector] public ModeManager modeManager;
    [HideInInspector]public Animator animator;
    public GameObject deathScreen;
    private Rigidbody rigidbody;

    [Space(20)]
    [Header("Weapon Vars")]
    /*
     * WEAPON VARS
     */
    protected float defaultFov = 60f;
    protected float zoomSpeed = 10f;
    protected float targetFOV = 60f;


    /*
     * HUD SCRIPTS
     */
    [Space(20)]
    [Header("HUD Vars")]
    public HUDScript hudScript;
    [SerializeField] protected GameObject settingsMenu;
    protected bool inSettings;
    public float defaultCameraShakeMagnitude;
    public float defaultCameraShakeDuration;

    /*
     * INVENTORY VARS
     */
    [Space(20)]
    [Header("Inventory Vars")]
    public GameObject inventory;
    protected Player_InventoryScript inventoryScript;
    protected bool inInventory = false;
    private bool menuIsOpen = false;

    /**
     * MOVEMENT VARS
     */
    [Space(20)]
    [Header("Movement Vars")]
    [SerializeField] protected float defaultMovementSpeed;
    #region Movment Vars
    protected bool canMove = true;
    protected bool canControlPlayer = true;
    

    [SerializeField] protected float groundDrag;
    [SerializeField] protected float airMovementSpeedMultiplier;
    protected float maxSpeed = 10f;
    [SerializeField] protected float jumpHeight;
    protected float forwardValue;
    protected float sideValue;
    protected bool canJump = true;
    public int jumpCount;

    protected Vector3 moveDir;
    protected float gravity = -9.81f;
    protected Vector3 velocity;
    public bool isGrounded;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool shouldControlSpeed;


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





    /*
    * GRAPPLING HOOK
    */
    [Space(10)]
    [SerializeField] private float grapplingHookSpeed;
    [SerializeField] private float grapplingHookRange;
    [SerializeField] private float grapplingHookForce;
    [SerializeField] private float grapplingSwingSpeed;
    [Space(10)]
    public LineRenderer grapplingHookLine;
    public GameObject grapplingHookObject;
    public LayerMask grapplingLayers;
    public Transform grapplingHookShootPoint;
    private bool isGrapplingActive;
    private bool isGrapplingShooting = false;
    private bool isGrapplingAttached = false;
    private bool isGrapplingRetracting = false;
    private Vector3 grapplePoint;
    private SpringJoint grappleJoint;



    protected bool ultimateAbilityUnlocked = false;

    [Space(10)]
    [Header("Ladder Vars")]
    public float ladderSpeed;
    private bool isOnLadder;

    [Space(20)]
    [Header("Ultimate Ability")]
    [SerializeField] private UltimateAbilityInformation ultimateAbilityInformation;
    protected bool canUseUltimateAbility = false;
    private ParentUltimateAbility ultimateAbilityScript;

    public UltimateAbilityInformation testUltimateAbility;

    [Space(10)]
    [Header("Special Ability")]
    [SerializeField] private SpecialAbilityInformation specialAbilityInformation;
    protected bool canUseSpecialAbility = false;
    private ParentSpecialAbility specialAbilityScript;

    public SpecialAbilityInformation testAbility;


    #endregion

    /**
     * RESOURCE VAR
     */
    [Space(20)]
    [Header("Resource Vars")]
    #region Resources Vars

    [SerializeField] protected int diamondAmount;

    [SerializeField] protected int soulFire;

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
        playerCamera = GetComponentInChildren<Camera>();
        modeManager = GetComponent<ModeManager>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;

        //upgradeTree.gameObject.SetActive(false);
        defaultFov = playerCamera.fieldOfView;
        maxSpeed = defaultMovementSpeed;
        health = maxHealth;
        hudScript.UpdateHealth();
        if (testAbility != null)
        {
            print("Giving ability");
            GiveSpecialAbility(testAbility);
        }

        if (testAbility != null)
        {
            print("Giving ability");
            GiveUltimateAbility(testUltimateAbility);
        }

    }

    // Update is called once per frame
    protected void Update()
    {
        /**
         * MOVEMENT REGION
         */
        #region BASIC MOVEMENT REGION

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {

            if (isOnLadder)
            {
                rigidbody.useGravity = false;
            }
            else
            {
                rigidbody.useGravity = true;
            }
            if (jumpCount > 0)
            {
                ResetJump();
            }


            canWallKick = true;
            rigidbody.drag = groundDrag;
            maxSpeed = defaultMovementSpeed;

        } else
        {
            rigidbody.drag = 0;
        }


        if (isSonicEffect)
        {
            if (sonicEffectTime + startSonicEffectTime < Time.time)
            {
                isSonicEffect = false;
                maxSpeed = defaultMovementSpeed;
            }
            maxSpeed = sonicMovementSpeed;
        }



        if (canMove)
        {

            forwardValue = Input.GetAxis("Vertical");
            sideValue = Input.GetAxis("Horizontal");

            //TODO FIX LADDER MOVEMENT
            if (!isOnLadder)
            {
                moveDir = forwardValue * transform.forward + sideValue * transform.right;
                //movementSpeedVar = defaultMovementSpeed;
            }
            else
            {
                moveDir = forwardValue * Vector3.up + sideValue * transform.right;
                maxSpeed = ladderSpeed;
            }
            
            if (!moveDir.Equals(Vector3.zero))
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);

            }
            #endregion

            if (Input.GetButtonDown("SpecialAbility"))
            {
                ActivateSpecialAbility();
            }

            if (Input.GetButtonDown("UltimateAbility"))
            {
                ActivateUltimateAbility();
            }


            if (Input.GetButtonDown("Jump"))
            {
                Jump();


            }


            /*
             * GRAPPLING HOOK
             */
            #region GRAPPLING HOOK
            if (Input.GetButtonDown("GrapplingHook") && !isGrapplingActive)
            {
                isGrapplingAttached = ShootGrapplingHook();
            }

            if (Input.GetButtonUp("GrapplingHook") && isGrapplingActive)
            {
                RetractGrapplingHook();
            }

            if (isGrapplingShooting)
            {

                if (Vector3.Distance(grapplingHookLine.GetPosition(1), grapplePoint) < .5f)
                {
                    if (isGrapplingAttached)
                    {
                        GrapplingAttached();
                    }
                    else
                    {
                        RetractGrapplingHook();
                    }

                }
            }

            if (isGrapplingRetracting)
            {
                grapplingHookLine.SetPosition(1, Vector3.MoveTowards(grapplingHookLine.GetPosition(1), grapplingHookShootPoint.position, Time.deltaTime * grapplingHookSpeed));
                if (Vector3.Distance(grapplingHookLine.GetPosition(1), grapplingHookShootPoint.position) < .5f)
                {
                    GrapplingStop();
                }
            }
        }

        #endregion

        if (isDead)
        {
            if (deathTime + respawnTime < Time.time)
            {
                Respawn();
            }
        }

        #region MENU REGION
        if (Input.GetButtonDown("Settings"))
        {
            inSettings = !inSettings;
            OpenMenu(inSettings);
            settingsMenu.SetActive(inSettings);
        }

        if (Input.GetButtonDown("Inventory"))
        {
            inInventory = !inInventory;
            
            OpenMenu(inInventory);
            inventory.SetActive(inInventory);
        }
        #endregion
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);

        SpeedControl();
    }

    public void LateUpdate()
    {
        if (isGrapplingActive)
        {
            grapplingHookLine.SetPosition(0, grapplingHookShootPoint.position);

            if (isGrapplingShooting)
            {
                grapplingHookLine.SetPosition(1, Vector3.MoveTowards(grapplingHookLine.GetPosition(1), grapplePoint, Time.deltaTime * grapplingHookSpeed * 2f));
            }
        }
    }

    public void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
        }
    }

    #region Grappling Hook Methods
    private bool ShootGrapplingHook()
    {

        isGrapplingAttached = false;
        isGrapplingShooting = true;
        isGrapplingRetracting = false;
        isGrapplingActive = true;
        grapplingHookObject.SetActive(true);
        grapplingHookLine.positionCount = 2;
        for (int i = 0; i < grapplingHookLine.positionCount; i++)
        {
            grapplingHookLine.SetPosition(i, transform.position);
        }


        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grapplingHookRange, grapplingLayers))
        {
            grapplePoint = hit.point;
            
            return true;
        }

        grapplePoint = playerCamera.transform.position + playerCamera.transform.forward * grapplingHookRange;
        return false;
    }


    private void GrapplingAttached()
    {
        isGrapplingShooting = false;
        shouldControlSpeed = false;

        grappleJoint = gameObject.AddComponent<SpringJoint>();
        grappleJoint.autoConfigureConnectedAnchor = false;
        grappleJoint.connectedAnchor = grapplePoint;

        float distance = Vector3.Distance(transform.position, grapplePoint);

        grappleJoint.maxDistance = distance * .6f;
        grappleJoint.minDistance = distance * .2f;

        //change these
        grappleJoint.spring = 4.5f;
        grappleJoint.damper = 8f;
        grappleJoint.massScale = 4.5f;

        maxSpeed = grapplingSwingSpeed;

    }

    private void RetractGrapplingHook()
    {
        isGrapplingShooting = false;
        isGrapplingRetracting = true;

        if(isGrapplingAttached)
            Destroy(grappleJoint);

        isGrapplingAttached = false;
    }

    private void GrapplingStop()
    {
        isGrapplingActive = false;
        isGrapplingRetracting = false;
        isGrapplingAttached = false;
        isGrapplingShooting = false;

        grapplingHookLine.positionCount = 0;
        grapplingHookObject.SetActive(false);
    }
    #endregion

    #region Basic Movement Methods

    private void MovePlayer()
    {
        if (!canControlPlayer)
        {
            return;
        }

        if (isGrapplingAttached)
        {
            return;
        }
        if (isGrounded)
        {
            rigidbody.AddForce(moveDir.normalized * maxSpeed * 10f, ForceMode.Force);
        } else
        {
            rigidbody.AddForce(moveDir.normalized * maxSpeed * 10f * airMovementSpeedMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelo = rigidbody.velocity.xz3();
        if (flatVelo.magnitude > maxSpeed)
        {

            Vector3 limitedVel = flatVelo.normalized * maxSpeed;

            rigidbody.velocity = new Vector3(limitedVel.x, rigidbody.velocity.y, limitedVel.z);
        }
        //fix damamge multiplier
        if (modeManager.GetPlayerMode() == PlayerMode.Weapons)
        {
            hudScript.damageMultiplier.gameObject.SetActive(true);
            float multiplier = Mathf.Lerp(1f, 2f, flatVelo.magnitude / grapplingSwingSpeed);
            multiplier = Mathf.Round(multiplier * 10f) / 10f;
            modeManager.GetEquipedWeapon().SetDamageMultiplier(multiplier);
            hudScript.UpdateDamageMultiplier(multiplier);
        } else
        {
            hudScript.damageMultiplier.gameObject.SetActive(false);
        }

    }

    private void Jump()
    {
        if (!canJump)
        {
            return;
        }

        if (jumpCount > 1)
        {
            canJump = false;
            return;
        }
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

        rigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);

        jumpCount++;

        animator.SetTrigger("isJumping");

    }

    private void ResetJump()
    {
        canJump = true;
        jumpCount = 0;
    }
    #endregion

    public void ActivateUltimateAbility()
    {
        if (!canUseUltimateAbility)
        {
            return;
        }

        if (ultimateAbilityInformation == null)
        {
            return;
        }

        ultimateAbilityScript.ActivateAbility();
        ultimateAbilityScript.DepleteCharge();
    }

    public void GiveUltimateAbility(UltimateAbilityInformation information)
    {
        if (ultimateAbilityScript != null)
        {
            ultimateAbilityScript.DestroyAbility();
        }
        ultimateAbilityInformation = information;

        System.Type classtype = information.functionalityScript.GetClass();
        ultimateAbilityScript = gameObject.AddComponent(classtype) as ParentUltimateAbility;



        hudScript.ChangeUltimateAbilityImage(information.icon);
        hudScript.UpdateUltimateAbilityCharge(0f);
        ultimateAbilityScript.SetChargeAmount(information.chargeAmount);
        canUseUltimateAbility = false;
    }

    public void ChargeUltimateAbility(float multiplier)
    {
        if(ultimateAbilityScript == null)
        {
            return;
        }

        canUseUltimateAbility = ultimateAbilityScript.ChargeUp(multiplier);

    }



    public void ActivateSpecialAbility()
    {
        if (!canUseSpecialAbility)
        {
            return;
        }

        if (specialAbilityInformation == null)
        {
            return;
        }
        specialAbilityScript.ActivateAbility();
        StartCoroutine(SpecialAbilityOnCooldown());

        
    }

    public void GiveSpecialAbility(SpecialAbilityInformation information)
    {
        if (specialAbilityScript != null)
        {
            specialAbilityScript.DestroyAbility();
        }
        specialAbilityInformation = information;
        System.Type classtype = information.functionalityScript.GetClass();
        specialAbilityScript = gameObject.AddComponent(classtype) as ParentSpecialAbility;
        hudScript.ChangeSpecialAbilityImage(information.icon);
        StartCoroutine(SpecialAbilityOnCooldown());
    }

    public IEnumerator SpecialAbilityOnCooldown()
    {

        float timer = 0f;
        canUseSpecialAbility = false;
        hudScript.UpdateSpecialAbilityCooldown(timer);
        while (timer <= specialAbilityInformation.cooldown)
        {
            hudScript.UpdateSpecialAbilityCooldown(timer / specialAbilityInformation.cooldown);
            timer += Time.deltaTime;
            yield return null;
        }
        canUseSpecialAbility = true;
        hudScript.UpdateSpecialAbilityCooldown(1f);

        yield return null;
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
        ChangeCameraZoom(1f);
        SetAllBoolsFalse();
        isDead = true;
        canMove = false;
        canJump = false;
        soulFire = soulFire / 2;
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
        health = maxHealth;
        isDead = false;
        ResetJump();
        canMove = true;
        hudScript.UpdateHealth();
        hudScript.UpdateSoulFireValues();

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

    public bool CanAffordResources(int diamondCost)
    {
        return diamondAmount >= diamondCost;
    }

    public bool CanAffordSoulFire(int cost)
    {
        return soulFire >= cost;
    }


    public void OpenMenu(bool isOpen)
    {
        if (isOpen && menuIsOpen)
        {
            return;
        }

        if (isOpen)
        {
            SetAllBoolsFalse();
            ChangeCameraZoom(1f);
            hudScript.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            lookScript.setCanLook(false);
            canMove = false;
            //weaponManager.canShoot(false);
            modeManager.SetFreeToPlay(false);
            menuIsOpen = true;
        }
        else
        {
            hudScript.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            lookScript.setCanLook(true);
            canMove = true;

            modeManager.SetFreeToPlay(true);
            menuIsOpen = false;
        }
    }

    /*
     * GET AND SET VARS
     */
    #region Get Set Functions

    public void ChangeDiamondAmount(int delta)
    {
        if (delta == 0)
        {
            return;
        }
        diamondAmount += delta;

        hudScript.UpdateResourceValues();
        hudScript.SpawnResoucesChangeIndicator(true, delta);
    }

    public int GetDiamondAmount()
    {
        return diamondAmount;
    }

    public Rigidbody GetRigidbody()
    {
        return rigidbody;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void SetMaxSpeed(float speed)
    {
        maxSpeed = speed;
    }

    public int GetSoulFire()
    {
        return soulFire;
    }

    public void SetSoulFire(int delta)
    {
        soulFire += delta;
        hudScript.UpdateSoulFireValues();
        hudScript.SpawnResoucesChangeIndicator(false, delta);
    }

    public void SetCanControlPlayer(bool value)
    {
        canControlPlayer = value;
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

    public void GiveDamage(float damage, Collider hitCollider, out float damageGiven, out bool crit)
    {
        GiveDamage(damage);
        damageGiven = damage;
        crit = false;
    }

    public void GiveDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;
            hudScript.UpdateHealth();
            lookScript.CameraShake(defaultCameraShakeDuration, defaultCameraShakeMagnitude);
            if (health <= 0)
            {
                Death();
            }
        }
    }


}

public enum SPECIAL_ABILITY
{
    NONE,
    DASH

}

public enum ULTIMATE_ABILITY
{
    NONE,
    GROUND_SLAM
}

public enum PLAYER_HINT
{
    USE,
    COST,
    ROTATE,
}

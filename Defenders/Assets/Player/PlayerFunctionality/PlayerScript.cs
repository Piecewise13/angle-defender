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
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    private float forwardValue;
    private float sideValue;
    private bool canJump;

    private Vector3 moveDir;
    private float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
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
            controller.Move(moveDir * movementSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && canJump)
            {

                canJump = false;
                velocity.y = jumpHeight;
                
                animator.SetTrigger("isJumping");
                animator.SetBool("isFalling", true);
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

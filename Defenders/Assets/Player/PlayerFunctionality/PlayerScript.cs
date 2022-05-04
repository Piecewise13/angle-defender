using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerScript : MonoBehaviour, Damageable
{
    public CharacterController controller;
    public Camera playerCamera;
    [SerializeField] private Transform groundCheck;
    private MouseLook lookScript;
    private WeaponInventoryManager weaponManager;

    /*
     * HUD SCRIPTS
     */
    public HUDScript hudScript;
    [SerializeField] private UpgradeTreeScript upgradeTree;

    /**
     * MOVEMENT VARS
     */
    #region
    private bool canMove = true;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    private float forwardValue;
    private float sideValue;

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
    #region
    [SerializeField] private int woodAmount;
    [SerializeField] private int ironAmount;
    [SerializeField] private int diamondAmount;

    public float health { get; set; }
    public bool isDead { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public float maxHealth;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        lookScript = GetComponentInChildren<MouseLook>();
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        weaponManager = GetComponent<WeaponInventoryManager>();

        upgradeTree.gameObject.SetActive(false);
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
        }
        if (canMove)
        {
            forwardValue = Input.GetAxis("Vertical");
            sideValue = Input.GetAxis("Horizontal");


            moveDir = forwardValue * transform.forward + sideValue * transform.right;

            controller.Move(moveDir * movementSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump"))
            {


                velocity.y = jumpHeight;
            }

        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        #endregion
    }

    public void updateResourceAmount(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:
                woodAmount += amount;
                break;
            case ResourceType.Iron:
                ironAmount += amount;
                break;

            case ResourceType.Diamond:
                diamondAmount += amount;
                break;

        }
        hudScript.UpdateResourceValues();
        hudScript.ResoucesChangeFade(type, amount);
    }

    public int getResourceAmount (ResourceType type){

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

    public void takeDamage(float damage, Collider hitCollider)
    {
        throw new System.NotImplementedException();
    }

    public void death() { }
}

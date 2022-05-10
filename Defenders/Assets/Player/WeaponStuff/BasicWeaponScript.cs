using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicWeaponScript : MonoBehaviour
{

    //Damage
    public float damage;
    public static float damageMultiplier = 1;
    public LayerMask layer;

    

    //Bullet Tracer 
    public TrailRenderer bulletTrail;
    public GameObject bulletSpawnPoint;

    //Shoot Speed Stuff
    public float shootDelay;
    protected float lastShootTime;
    [SerializeField] protected float weaponSetUpTime;
    protected bool setUp = true;

    //Recoil Vars
    protected Camera playerCamera;
    public GameObject cameraRotator;
    [SerializeField] protected Vector3 recoilAmount;
    protected Vector3 targetRotation;
    protected Vector3 currentRotation;
    [SerializeField] protected float snappiness;
    [SerializeField] protected float returnSpeed;

    //Reload and Bullet Stuff
    public int clipSize;
    protected int currentNumOfBullets;
    protected static int bulletsCarried = 100;
    [SerializeField]protected int bulletCost;
    public AnimatorOverrideController gunAnims;
    protected static float reloadDuration = 2.5f;
    protected bool isReloading;
    protected float startReloadTime; 

    protected static HUDScript hud;
    protected Animator playerAnimator;
    protected static PlayerScript player;

    protected float setupTimer;
    

    protected bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = gameObject.GetComponentInParent<Camera>();
        playerAnimator = GetComponentInParent<Animator>();
        hud = GameObject.FindObjectOfType<HUDScript>();
        UpdateHUD();
        cameraRotator = playerCamera.gameObject.transform.parent.gameObject;
        currentNumOfBullets = clipSize;
        EquipGun();
    }

    // Update is called once per frame
    protected void ControlRecoil()
    {

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        cameraRotator.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    protected void Recoil()
    {
        targetRotation += new Vector3(recoilAmount.x,
    Random.Range(-recoilAmount.y, recoilAmount.y),
    Random.Range(-recoilAmount.z, recoilAmount.z));
    }

    public void setCanShoot(bool value)
    {
        canShoot = value;
    }

    public void Reload()
    {
       
        bulletsCarried -= (clipSize - currentNumOfBullets) * bulletCost;
        currentNumOfBullets = clipSize;
        canShoot = false;
        isReloading = true;
        startReloadTime = Time.time;
        playerAnimator.SetTrigger("reloadGun");
        UpdateHUD();
    }

    protected void UpdateHUD()
    {
        hud.UpdateBulletValues(clipSize, currentNumOfBullets);

    }


    public abstract void EquipGun();

    public static void ChangeBulletsCarried(int amount)
    {
        
        bulletsCarried += amount;
        hud.UpdateBulletValues();

    }

    public static int NumBulletsCarried()
    {
        return bulletsCarried;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicWeaponScript : MonoBehaviour
{

    //Damage
    [Space(20)]
    [Header("Damage Vars")]
    public float damage;
    public static float damageMultiplier = 1;
    public LayerMask layer;

    

    //Bullet Tracer
    [Space(20)]
    [Header("Bullet Tracer Vars")]
    public TrailRenderer bulletTrail;
    public GameObject bulletSpawnPoint;
    protected TrailRenderer trailObject;

    //Shoot Speed Stuff
    [Space(20)]
    [Header("Shooting Vars")]
    public float shootDelay;
    protected float lastShootTime;
    [SerializeField] protected float weaponSetUpTime;
    protected bool setUp = true;

    [SerializeField] protected float adsZoom;

    //Recoil Vars
    [Space(20)]
    [Header("Recoil Vars")]
    protected Camera playerCamera;
    public GameObject cameraRotator;
    [SerializeField] protected Vector3 recoilAmount;
    protected Vector3 targetRotation;
    protected Vector3 currentRotation;
    [SerializeField] protected float snappiness;
    [SerializeField] protected float returnSpeed;

    //Reload and Bullet Stuff
    [Space(20)]
    [Header("Bullet Vars")]
    public int clipSize;
    protected int currentNumOfBullets;

    [SerializeField]protected int bulletCost;
    public AnimatorOverrideController gunAnims;
    [SerializeField] protected float reloadDuration;
    protected bool isReloading;
    protected float startReloadTime;

    [Space(20)]
    [Header("Component Vars")]
    public Sprite weaponIcon;
    protected static HUDScript hud;
    protected Animator playerAnimator;
    protected PlayerScript player;
    protected WeaponInventoryManager inventory;

    protected float setupTimer;
    

    protected static bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = gameObject.GetComponentInParent<Camera>();
        playerAnimator = GetComponentInParent<Animator>();
        player = GetComponentInParent<PlayerScript>();
        hud = GameObject.FindObjectOfType<HUDScript>();
        inventory = GetComponentInParent<WeaponInventoryManager>();
        currentNumOfBullets = clipSize;
        cameraRotator = playerCamera.gameObject.transform.parent.gameObject;
        

        EquipGun();
    }


    // Update is called once per frame
    protected void ControlRecoil()
    {

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        cameraRotator.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    protected void AddRecoil()
    {
        targetRotation += new Vector3(recoilAmount.x,
    Random.Range(-recoilAmount.y, recoilAmount.y),
    Random.Range(-recoilAmount.z, recoilAmount.z));
    }

    public static void SetCanShoot(bool value)
    {
        canShoot = value;
    }

    protected void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
        {
            trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
            //print("hit gameobject: " + hit.collider.gameObject);
            StartCoroutine(SpawnTrail(trailObject, hit.point));
            try
            {

                Damageable hitGameobject = hit.collider.gameObject.GetComponentInParent<Damageable>();
                //print("damageable gameobject: " + hitGameobject);


                hitGameobject.TakeDamage(damage * (1 + damageMultiplier), hit.collider);
                //print(damage * damageMultiplier);

            }
            catch (System.Exception)
            {

            }


        }
        else
        {

            trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
            //print("hit gameobject: " + hit.collider.gameObject);
            StartCoroutine(SpawnTrail(trailObject, playerCamera.transform.position + playerCamera.transform.forward * 500f ));
        }


        lastShootTime = Time.time;
        AddRecoil();
        currentNumOfBullets -= 1;


    }

    public virtual void Reload()
    {
        StopAim();
        int numBullets = player.GetSoulFire();
        if (numBullets <= 0)
        {
            numBullets = 0;
            return;
        }
        if (numBullets < bulletCost)
        {
            return;
        }
       if ((clipSize - currentNumOfBullets) * bulletCost > numBullets)
        {
            currentNumOfBullets += numBullets / bulletCost;
            player.SetSoulFire(-numBullets + (numBullets % bulletCost));
        } else
        {
            player.SetSoulFire(-1 * (clipSize - currentNumOfBullets) * bulletCost);
            currentNumOfBullets = clipSize;
        }

        canShoot = false;
        isReloading = true;
        startReloadTime = Time.time;
        playerAnimator.SetBool("isReloading", true);



    }

    protected virtual void FinishReload()
    {
        if (reloadDuration + startReloadTime > Time.time)
        {
            return;
        }
        playerAnimator.SetBool("isReloading", false);
        isReloading = false;
        canShoot = true;

    }


    public IEnumerator SpawnTrail(TrailRenderer trail, Vector3 point)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        //animator.SetBool("isShooting", false);
        trail.transform.position = point;
        //Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(trail.gameObject, trail.time);

    }





    public void SetPlayer(PlayerScript script)
    {
        player = script;
    }

    protected void StartAim()
    {
        playerAnimator.SetBool("isAiming", true);

        //player.ChangeCameraZoom(adsZoom);
    }

    protected void StopAim()
    {
        playerAnimator.SetBool("isAiming", false);
        player.lookScript.bUseAimSens = false;
        player.ChangeCameraZoom(1f);
    }

    private void OnDestroy()
    {
        StopAim();
        if (trailObject != null)
        {
            Destroy(trailObject.gameObject);

        }

    }

    public abstract void EquipGun();
}

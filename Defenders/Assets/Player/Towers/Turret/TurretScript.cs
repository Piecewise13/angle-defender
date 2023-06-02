using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : TowerParentScript
{
    public Animator anim;
    
    //bullet trail
    public TrailRenderer bulletTrail;
    public Transform bulletSpawnPoint;
    protected TrailRenderer trailObject;

    //Model Parts
    public GameObject barrelModel;
    [SerializeField]private GameObject[] barrelObjects;

    public GameObject turretModel;
    [SerializeField]private GameObject turretObject;


    private GameObject baseModel;
    [SerializeField]private GameObject baseObject;
    public Transform turretParent;

    private Transform[] spawnPoints;

    public LayerMask layerMask;
    private ParentAIScript target;
    [SerializeField] private float searchTime;
    private float lastSearchTime;

    private bool isShooting;

    [Header("TURRET STATS")]
    [SerializeField] private float targetRange;
    [SerializeField] private float baseDamage;
    private float damageMultiplier = 1f;
    [SerializeField] private float shootSpeedMultiplier = 1f;


    private Transform[] barrelLocation = new Transform[1];



    // Start is called before the first frame update
    void Start()
    {
        //base.Start();
        baseModel = baseObject;
    }




    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
        {
            return;
        }
        //print(target);
        if (target != null)
        {
            anim.SetBool("isShooting", isShooting);

            turretObject.transform.rotation = Quaternion.LookRotation(((target.transform.position + (Vector3.up * 2f)) - turretObject.transform.position).normalized);
            /*
            if (forge.fireStored > 0)
            {
                anim.SetBool("isShooting", isShooting);
                turret.transform.rotation = Quaternion.LookRotation(((target.transform.position + (Vector3.up * 2f)) - turret.transform.position).normalized);
            } else
            {

                particles.Stop();
            }
            */

        }
        else
        {
            anim.SetBool("isShooting", false);
            isShooting = false;
            if (lastSearchTime + searchTime < Time.time)
            {
                FindTarget();
                lastSearchTime = Time.time;
                
            }
        }

    }

    private void FindTarget()
    {
        
        Collider[] foundEnemy = Physics.OverlapSphere(transform.position, targetRange, layerMask);

        if (foundEnemy.Length == 0)
        {
            isShooting = false;
            print("found none");
            target = null;
            return;
        }

        float shortestDist = float.MaxValue;
        int current = -1;

        for (int i = 0; i < foundEnemy.Length; i++)
        {
            float distace = Vector3.Distance(foundEnemy[i].transform.position, transform.position);

            if(distace < shortestDist)
            {
                current = i;
                shortestDist = distace;
            }
        }

        print("Found Target");
        target = foundEnemy[current].GetComponentInParent<ParentAIScript>();
        isShooting = true;


    }

    public void UpgradeShootSpeed(float value)
    {
        shootSpeedMultiplier = value;
        //anim.SetFloat("shootSpeed", shootSpeedMultiplier);
    }

    
    public void Shoot()
    {
        if (target == null)
        {
            return;
        }
        target.GiveDamage(baseDamage * damageMultiplier);
        trailObject = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trailObject, target.transform.position + Vector3.up * 1.5f));
    }

    public void ChangeBarrels(GameObject model)
    {
        barrelModel = model;
        for (int i = 0; i < barrelObjects.Length; i++)
        {
            Destroy(barrelObjects[i]);
        }
        //print(turretObject);
        Transform[] barrelPoints = FindSpawnPoints(turretObject).ToArray();
        barrelObjects = new GameObject[barrelPoints.Length];
        //print(barrelPoints.Length);

        for (int i = 0; i < barrelObjects.Length; i++)
        {
            barrelObjects[i] = Instantiate(model, barrelPoints[i]);
            
        }
        //print(barrelObjects.Length);
        bulletSpawnPoint = FindSpawnPoints(barrelObjects[0])[0];
    }

    public void ChangeBody(GameObject model)
    {
        print("Changing Body");
        //get rid of old model
        DestroyImmediate(turretObject);
        //print(turretObject);
        turretModel = model;
        turretObject = Instantiate(model, turretParent);
        SetAnimator(turretObject.GetComponent<Animator>());
        ChangeBarrels(barrelModel);


    }

    public void ChangeBase(GameObject model)
    {
        print("Changing Base");
        Destroy(baseObject);
        baseModel = model;
        baseObject = Instantiate(model, transform);
        turretParent = FindSpawnPoints(baseObject)[0];
        ChangeBody(turretModel);
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

    public void SetAnimator(Animator anim)
    {
        this.anim = anim;
        anim.speed = shootSpeedMultiplier;
        //print(shootSpeedMultiplier + " " + anim.speed);
    }




    public void SetDamageMultiplier(float value)
    {
        damageMultiplier = value;
    }

    public void SetSeachRadius(float value)
    {

    }

    public void SetSpeedMultiplier(float value)
    {
        shootSpeedMultiplier = value;
    }


}

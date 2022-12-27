using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : TowerParentScript
{

    public GameObject turret;
    public Animator anim;
    
    //bullet trail
    public TrailRenderer bulletTrail;
    public GameObject bulletSpawnPoint;
    protected TrailRenderer trailObject;



    public LayerMask layerMask;
    private ParentAIScript target;
    [SerializeField] private float searchTime;
    private float lastSearchTime;

    private bool isShooting;

    [Header("TURRET STATS")]
    [SerializeField] private float targetRange;
    [SerializeField] private float damage;
    [SerializeField] private float shootSpeedMultiplier = 1f;






    // Start is called before the first frame update
    void Start()
    {

    }




    // Update is called once per frame
    void Update()
    {
        
        if (target != null)
        {

            anim.SetBool("isShooting", isShooting);
            turret.transform.rotation = Quaternion.LookRotation(((target.transform.position + (Vector3.up * 2f)) - turret.transform.position).normalized);

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
        //anim.SetFloat("shootSpeed", shootSpeedMultiplier);
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
        
        target = foundEnemy[current].GetComponentInParent<ParentAIScript>();
        isShooting = true;


    }

    public void UpgradeShootSpeed(float value)
    {
        shootSpeedMultiplier = value;
        
    }


    public void Shoot()
    {
        target.TakeDamage(damage, null);
        trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trailObject, target.transform.position + Vector3.up * 1.5f));

        /*
        if (forge.WithdrawFire(1))
        {
            particles.Play();
            target.TakeDamage(damage, null);
            trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trailObject, target.transform.position + Vector3.up  * 1.5f));
        } else
        {
            particles.Stop();
        }

        */
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
    }

}

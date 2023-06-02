using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTowerScript : MonoBehaviour
{
    [Header("Component Vars")]
    public GameObject turret;
    private Animator anim;

    //bullet trail
    [Header("Bullet Tracer")]
    public TrailRenderer bulletTrail;
    public GameObject bulletSpawnPoint;
    protected TrailRenderer trailObject;



    public LayerMask layerMask;
    public ParentAIScript target;
    [SerializeField] private float searchTime;
    private float lastSearchTime;

    private bool isShooting;
    public int bulletsLeft;

    [Header("TURRET STATS")]
    [SerializeField] private float targetRange;
    [SerializeField] private float damage;

    private bool isReady = true;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (target != null)
            {
                if (bulletsLeft > 0)
                {
                    anim.SetBool("isShooting", isShooting);
                    turret.transform.rotation = Quaternion.LookRotation(((target.transform.position) - turret.transform.position).normalized);
                    //Quaternion.AngleAxis
                }

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
        print(foundEnemy[0].transform.root.gameObject);
        float shortestDist = float.MaxValue;
        int current = -1;

        for (int i = 0; i < foundEnemy.Length; i++)
        {
            float distace = Vector3.Distance(foundEnemy[i].transform.position, transform.position);

            if (distace < shortestDist)
            {
                current = i;
                shortestDist = distace;
            }
        }

        target = foundEnemy[current].GetComponentInParent<ParentAIScript>();
        isShooting = true;


    }

    public void Shoot()
    {
        if (bulletsLeft > 0)
        {
            if (target == null)
            {
                isShooting = false;
                return;
            }
            target.GiveDamage(damage);
            trailObject = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trailObject, target.transform.position + Vector3.up * 1.5f));
            bulletsLeft--;
        } else
        {
            Destroy(gameObject);
        }


    }

    public void Setup()
    {
        isReady = true;
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
}

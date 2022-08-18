using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinaMonsterScript : PlayerBasedAIParent
{

    [Header("Axe Vars")]
    public GameObject axeObject;
    public GameObject axePrefab;
    [SerializeField] private float axeThrowDistace;

    [Space(20)]
    [Header("Rock Slam")]
    public GameObject rockPrefab;
    [SerializeField] int numOfRocks;
    public float rockSpawnDistance;
    public ParticleSystem magicParticles;

    [Space(20)]
    [Header("Rush")]
    public ParticleSystem[] trackParticle;
    public ParticleSystem hornParticle;
    private bool isRushing = false;
    private float rushSpeed;
    private Vector3 rushTarget;




    public float attackTime;
    private float lastAttackTime;


    private Animator anim;



    // Start is called before the first frame update
    new void Start()
    {
        
        base.Start();
        player = ClosestPlayer(transform.position);
        anim = GetComponentInChildren<Animator>();
        lastAttackTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTime + lastAttackTime < Time.time && !isRushing)
        {
            Attack();
        }

        if (isRushing)
        {
            if (agent.remainingDistance < .001f)
            {
                EndRush();
            }
        }

    }

    private void Attack()
    {

        float pValue = Random.value;
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 10f, player.transform.position, out hit, playerMask))
        {
            print(hit.collider.gameObject);
            lastAttackTime = Time.time - attackTime / 2;
            return;

        }

        rushTarget = transform.position + (player.transform.position -  transform.position).normalized * (Vector3.Distance(player.transform.position, transform.position) + 40f);
        print(("rush: " + rushTarget + " player: " + player.transform.position));
        if (!agent.SetDestination(rushTarget))
        {
            print("didn't works");
            return;
        }
        StartCoroutine(LookAtPoint(rushTarget, 1f));
        anim.SetBool("isRushing", true);



        if (pValue <= 1/3)
        {
            //if (Physics.Linecast(transform.position, player.transform.position, playerMask))
            //{

            //    lastAttackTime = Time.time - attackTime / 2;
            //    return;

            //}

            //StartCoroutine(LookAtPoint(player.transform.position, 1f));

            //anim.SetTrigger("throwAxe");


            //axe
        } else if (pValue >= 2/3)
        {


            //magicParticles.Play();

            //anim.SetTrigger("rockSlam");
            //for (int i = 0; i < numOfRocks; i++)
            //{
            //    Vector3 spawnPos = transform.position + (Quaternion.Euler(0f, i * (360 / numOfRocks), 0f) * transform.forward) * rockSpawnDistance;
            //    SlamRockScript rockScript = Instantiate(rockPrefab, spawnPos + Vector3.down * 5, Quaternion.Euler(Vector3.zero)).GetComponent<SlamRockScript>();
            //    rockScript.SetMinaPos(transform.position);
            //}
        } else
        {




        }
        lastAttackTime = Time.time;


    }


    public IEnumerator LookAtPoint(Vector3 point, float dur)
    {
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(point - transform.position);
        float t = 0f;
        while(t< dur)
        {
            transform.rotation = Quaternion.Slerp(start, end, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
    }



    public void AxeThrow()
    {
        print("throwingAxe from anim");
        MinaAxeScript axe = Instantiate(axePrefab, axeObject.transform.position, Quaternion.LookRotation(transform.forward)).GetComponent<MinaAxeScript>();
        axe.SetThrowVars(player.gameObject.transform.position);
        axeObject.SetActive(false);
    }

    public void GrabNewAxe()
    {
        print("grabbing new axe");
        axeObject.SetActive(true);
    }

    public void RocksStartMove()
    {
        SlamRockScript.bShouldMove = true;
    }

    public void SlamRocks()
    {
        SlamRockScript.shouldSlam = true;
    }


    public void StartRush()
    {
        TrackParticlesPlay(true);
        rushTarget = transform.position + (player.transform.position - transform.position).normalized * (Vector3.Distance(player.transform.position, transform.position) + 40f);
        StartCoroutine(LookAtPoint(rushTarget, .5f));
        agent.SetDestination(rushTarget);
        agent.isStopped = true;

        isRushing = true;

        print(("rush: " + rushTarget + " player: " + player.transform.position));

    }

    public void Rush()
    {
        agent.isStopped = false;
        agent.acceleration = 40f;
        agent.speed = 100f;
    }

    public void EndRush()
    {
        print("rush ended");
        anim.SetBool("isRushing", false);
        agent.speed = 10f;
        agent.acceleration = 8f;
        TrackParticlesPlay(false);
    }

    private void TrackParticlesPlay(bool play)
    {
        if (play)
        {
            foreach (var item in trackParticle)
            {
                item.Play();
            }
        } else
        {
            foreach (var item in trackParticle)
            {
                item.Stop();
            }
        }
    }



    public override void PlayerFound(PlayerScript player)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerLost(PlayerScript player)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float damage, Collider hitCollider)
    {
        throw new System.NotImplementedException();
    }

    public override void Death()
    {
        throw new System.NotImplementedException();
    }
}

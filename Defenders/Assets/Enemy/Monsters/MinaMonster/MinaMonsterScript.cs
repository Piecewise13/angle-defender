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
    public GameObject rushTrigger;



    private bool canAttack = true;
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
        rushTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTime + lastAttackTime < Time.time && !isRushing && canAttack)
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
        agent.isStopped = true;
        float pValue = Random.value;



        print("p value: " + pValue);
        if (pValue <= 1f/3f)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.transform.position, out hit, playerMask))
            {
                print(hit.collider.gameObject);
                lastAttackTime = Time.time - attackTime / 2;
                return;

            }
            print("throwing axe");
            StartCoroutine(LookAtPoint(player.transform.position, 1f));

            anim.SetTrigger("throwAxe");
            canAttack = false;


            //axe
        } else if (pValue > 2f/3f)
        {

            print("Rock Slam");
            magicParticles.Play();

            anim.SetTrigger("rockSlam");
            for (int i = 0; i < numOfRocks; i++)
            {

                Vector3 spawnPos = transform.position + (Quaternion.Euler(0f, i * (360 / numOfRocks), 0f) * transform.forward) * rockSpawnDistance;
                SlamRockScript rockScript = Instantiate(rockPrefab, spawnPos + Vector3.down * 5, Quaternion.Euler(Vector3.zero)).GetComponent<SlamRockScript>();
                rockScript.SetMinaPos(transform.position);
            }
            canAttack = false;
        }
        else
        {

            RaycastHit hit;
            if (Physics.Linecast(transform.position + Vector3.up * 10f, player.transform.position, out hit, playerMask))
            {
                print(hit.collider.gameObject);
                lastAttackTime = Time.time - attackTime / 2;
                return;

            }

            rushTarget = transform.position + (player.transform.position - transform.position).normalized * (Vector3.Distance(player.transform.position, transform.position) + 40f);
            print(("rush: " + rushTarget + " player: " + player.transform.position));

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(rushTarget, path);
            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                print("didn't works");
                return;
            }
            print("Rushing");
            StartCoroutine(LookAtPoint(rushTarget, 1f));
            anim.SetBool("isRushing", true);
            canAttack = false;

        }




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
        canAttack = true;
        lastAttackTime = Time.time;
        MoveToPlayer();
    }

    public void RocksStartMove()
    {
        SlamRockScript.bShouldMove = true;
    }

    public void SlamRocks()
    {
        SlamRockScript.shouldSlam = true;
        canAttack = true;
        lastAttackTime = Time.time;
        magicParticles.Stop();
        MoveToPlayer();
    }


    public void StartRush()
    {
        TrackParticlesPlay(true);
        rushTarget = transform.position + (player.transform.position - transform.position).normalized * (Vector3.Distance(player.transform.position, transform.position) + 40f);
        StartCoroutine(LookAtPoint(rushTarget, .5f));
        agent.destination =  (rushTarget);
        agent.isStopped = true;


        canAttack = false;

        print(("rush: " + rushTarget + " player: " + player.transform.position));

    }

    public void Rush()
    {
        rushTrigger.SetActive(true);
        isRushing = true;
        agent.isStopped = false;
        agent.acceleration = 40f;
        agent.speed = 100f;
    }

    public void EndRush()
    {
        print("rush ended");
        rushTrigger.SetActive(false);
        anim.SetBool("isRushing", false);
        agent.speed = 10f;
        agent.acceleration = 8f;
        MoveToPlayer();
        TrackParticlesPlay(false);
        isRushing = false;
        canAttack = true;
        lastAttackTime = Time.time;
    }

    private void MoveToPlayer()
    {
        agent.isStopped = false;
        player = ClosestPlayer();
        agent.destination = player.transform.position;
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
}

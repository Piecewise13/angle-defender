using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingDemonScript : PlayerBasedAIParent
{
    [Header("Misc Vars")]
    public Animator anim;
    private PlayerDataMangerScript playerData;

    private bool canAttack = true;
    [SerializeField] private float attackTime;
    private float lastAttackTime;

    [Header("Movement Vars")]
    [SerializeField] private float defaultMoveSpeed;
    private Vector3 moveGoal;
    public float moveSpeed;
    public float rotationSpeed;
    private bool shouldMove;


    [Header("Circle Vars")]
    [Range(2, 20)]
    public int circleQuality;
    public float circleRadius;
    public float circleHeight;
    private bool isCircling;
    [SerializeField] private float circleCheckTime;
    private float lastCircleCheck;
    private int currentInterval;


    [Space(20)]
    [Header("Sonic Attack Vars")]
    public GameObject sonicAttackPrefab;
    public Transform sonicAttackSpawn;
    private bool shouldBeSonicAttack;
    private bool isSonicAttack;
    public float sonicHeight;
    public float sonicDistance;




    [Space(20)]
    [Header("Bombing Run Vars")]
    //Bombing Run Vars
    public GameObject bombPrefab;
    public Transform bombSpawn;
    private bool shouldStartBombing;
    private bool isBoming;
    [SerializeField] float bombDropTime;
    private float lastBombDropTime;
    [SerializeField] private float bombingSpeed;
    [SerializeField] private float bombHeight;
    [SerializeField] private float bombDistance;


    [Space(20)]
    [Header("Wall Attack Vars")]
    public float wallAttackSpeed;
    public float wallAttackHeight;
    public float wallAttackDistance;
    private bool shouldBeWallAttack;
    private bool isWallAttack;
    private Vector3 wallInitPos;
    [SerializeField] private float wallDamage;

 


    new private void Start()
    {
        base.Start();
        moveSpeed = defaultMoveSpeed;
        shouldMove = true;
        anim = GetComponentInChildren<Animator>();
        StartCircle();
        playerData = FindObjectOfType<PlayerDataMangerScript>();
    }
    // Update is called once per frame
    void Update()
    {

        if (isCircling)
        {
            if (circleCheckTime + lastCircleCheck < Time.time)
            {
                Circle();
                lastCircleCheck = Time.time;
            }
        }

        if (canAttack)
        {
            if (attackTime + lastAttackTime < Time.time)
            {
                Attack();
            }
        }

        if (shouldBeSonicAttack)
        {
            if (Vector3.Distance(transform.position, moveGoal) < 5f)
            {
                print("start attacking");
                anim.SetBool("isSonicAttack", true);
                shouldMove = false;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((player.transform.position - transform.position).normalized), rotationSpeed);
            }
        }


        if (shouldStartBombing)
        {

            if (Vector3.Distance(transform.position, moveGoal) < 3f)
            {
                anim.SetBool("isBombing", true);
                shouldMove = false;

            }

            if (!shouldMove)
            {
                StartBombing();
            }

        }

        if (isBoming)
        {
            float remainingDist = Vector3.Distance(transform.position, moveGoal);
            if (remainingDist > 5f)
            {
                if (lastBombDropTime + bombDropTime < Time.time)
                {
                    Instantiate(bombPrefab, bombSpawn.position, Quaternion.Euler(Vector3.zero));
                    lastBombDropTime = Time.time;
                }
            }
            else
            {
                StopBombing();
            }


        }

        if (shouldBeWallAttack)
        {
            if (Vector3.Distance(transform.position, moveGoal) < 5f)
            {
                anim.SetBool("isWallAttack", true);
                StartWallAttack();
                shouldMove = false;

            }
        }

        if (isWallAttack)
        {
            if (Vector3.Distance(transform.position, moveGoal) < 2f)
            {
                EndWallAttack();
            }
        }




        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((moveGoal - transform.position).normalized), rotationSpeed);
        if (shouldMove)
        {
            transform.position += (transform.forward * moveSpeed * Time.deltaTime);
        }


    }

    #region Circle Region
    private void StartCircle()
    {
        shouldMove = true;
        isCircling = true;
        moveSpeed = defaultMoveSpeed;
        print("circling started");
        currentInterval = 0;
        float angle = Quaternion.Angle(Quaternion.LookRotation(Vector3.forward, Vector3.up), Quaternion.LookRotation((transform.position - Vector3.zero).normalized, Vector3.up));

        float shortestDist = float.MaxValue;
        for (int i = 0; i < circleQuality / 2; i++)
        {
            float checkAngle = (360 / circleQuality) * i;
            float distance = Mathf.Abs(angle - checkAngle);
            if (distance < shortestDist)
            {
                shortestDist = distance;

                currentInterval = i;
            }
        }


        if (transform.position.x <= 0)
        {
            currentInterval += currentInterval / 2;
        }



        SetNewMoveGoal((Vector3.up * circleHeight) + (Quaternion.Euler(0f, (360 / circleQuality) * currentInterval, 0f) * Vector3.forward) * circleRadius);
        return;




    }

    private void Circle()
    {
        float distanceToPoint = Vector3.Distance(transform.position, moveGoal);
        if (distanceToPoint <= 10f)
        {
            currentInterval++;
        }

        SetNewMoveGoal((Vector3.up * circleHeight) + (Quaternion.Euler(0f, (360 / circleQuality) * currentInterval, 0f) * Vector3.forward) * circleRadius);
    }

    private void EndCircle()
    {
        isCircling = false;

    }

    #endregion

    private void SetNewMoveGoal(Vector3 pos)
    {
        moveGoal = pos;
    }




    private void Attack()
    {
        /*three attacks
         * 
         * 1:Sonic Scrambler - shoots out a ray that deafens player and makes them considerably slower 
         * 2: Bombing Run
         * 3:Wall Attack
         *  
        */



        float pValue = Random.value;


        if (pValue <= 1f / 3f)
        {
            EndCircle();
            //boming run first
            player = GetClosestPlayer();
            SetNewMoveGoal((player.transform.position - (Extns.xz3(player.transform.position) - Extns.xz3(transform.position)).normalized * bombDistance) + (Vector3.up * bombHeight));
            canAttack = false;
            shouldStartBombing = true;
        }
        else if (pValue > 1f / 3f && pValue <= 2f / 3f)
        {

            player = GetClosestPlayer();
            SetNewMoveGoal((player.transform.position - (Extns.xz3(player.transform.position) - Extns.xz3(transform.position)).normalized * sonicDistance) + (Vector3.up * sonicHeight));
            shouldBeSonicAttack = true;
        }
        else
        {
            targetWall = GetRandomWall();
            EndCircle();
            SetNewMoveGoal((targetWall.transform.position - (Extns.xz3(targetWall.transform.position) - Extns.xz3(transform.position)).normalized * wallAttackDistance) + (Vector3.up * wallAttackHeight));
            shouldBeWallAttack = true;
            canAttack = false;
        }



    }

    public void StartWallAttack()
    {
        SetNewMoveGoal(targetWall.transform.position);
        wallInitPos = transform.position;
        StartCoroutine(LookAtPoint(targetWall.transform.position, 1f));
        shouldMove = false;
    }

    public void WallAttack()
    {
        print("stat attacking");
        SetNewMoveGoal(targetWall.transform.position + (Vector3.up * 5f));
        moveSpeed = wallAttackSpeed;
        shouldMove = true;
        isWallAttack = true;
        shouldBeWallAttack = false;
    }

    public void EndWallAttack()
    {
        targetWall.GiveDamage(wallDamage);
        anim.SetBool("isWallAttack", false);
        print("end attack");
        SetNewMoveGoal((targetWall.transform.position + (Extns.xz3(targetWall.transform.position) - Extns.xz3(wallInitPos)).normalized * wallAttackDistance / 2f) + (Vector3.up * wallAttackHeight));
        canAttack = true;
        isWallAttack = false;
        lastAttackTime = Time.time;
        StartCoroutine(WaitToCircle(1.5f));
    }



    #region Sonic Attack
    public void SonicAttack()
    {
        Instantiate(sonicAttackPrefab, sonicAttackSpawn.position, Quaternion.Euler(Vector3.zero)).GetComponent<SonicAttackScript>().target = player.transform.position;
        isSonicAttack = true;

    }

    public void EndSonicAttack()
    {
        lastAttackTime = Time.time;
        canAttack = true;
        shouldBeSonicAttack = false;
        anim.SetBool("isSonicAttack", false);
        StartCircle();
    }
    #endregion


    #region Bombing Attack
    private void StartBombing()
    {
        //anim play bombing anim
        print("Start boming");
        SetNewMoveGoal((player.transform.position + (Extns.xz3(player.transform.position) - Extns.xz3(transform.position)).normalized * (bombDistance/2f)) + (Vector3.up * bombHeight));
        shouldMove = false;
    }

    public void Bombing()
    {
        print("bombs aways");

        moveSpeed = bombingSpeed;
        isBoming = true;
        shouldMove = true;
        shouldStartBombing = false;
    }

    private void StopBombing()
    {
        shouldStartBombing = false;
        isBoming = false;
        anim.SetBool("isBombing", false);
        canAttack = true;
        StartCircle();
        lastAttackTime = Time.time;
    }

    #endregion




    public IEnumerator LookAtPoint(Vector3 point, float dur)
    {
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(point - transform.position);
        float t = 0f;
        while (t < dur)
        {
            transform.rotation = Quaternion.Slerp(start, end, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
    }
    public IEnumerator WaitToCircle(float dur)
    {
        yield return new WaitForSeconds(dur);
        StartCircle();
    }

    public new void Death()
    {
        playerData.MonsterKilled();
        Destroy(gameObject);
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

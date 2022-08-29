using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingDemonScript : PlayerBasedAIParent
{

    private Vector3 moveGoal;
    [SerializeField] private float defaultMoveSpeed;
    private float moveSpeed;


    [Header("Circle Vars")]
    [Range(2, 20)]
    public int circleQuality;
    private bool isCircling = true;
    [SerializeField] private float circleCheckTime;
    private float lastCircleCheck;


    private bool canAttack = false;
    [SerializeField] private float attackTime;
    private float lastAttackTime;


    [Space(20)]
    [Header("Bombing Run Vars")]
    //Bombing Run Vars
    public GameObject bombPrefab;
    public Transform bombSpawn;
    private bool shouldStartBombing;
    private bool isBoming;
    [SerializeField] int numToDrop;
    private int numDropped;
    [SerializeField] private float bombingSpeed;
    [SerializeField] private float bombHeight;
    [SerializeField] private float bombDistance;
    private float remainingDistance;
 


    new private void Start()
    {
        base.Start();
        moveSpeed = defaultMoveSpeed;
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

        if (shouldStartBombing)
        {
            if (Vector3.Distance(transform.position, moveGoal) < 1f)
            {
                StartBombing();
            }
        }

        if (isBoming)
        {

            float remainingDist = Vector3.Distance(transform.position, moveGoal);
            if (remainingDist < ((bombDistance * 2) / numToDrop) * (numToDrop - numDropped))
            {
                Instantiate(bombPrefab, bombSpawn.position, bombSpawn.rotation);
                numDropped++;
                if (numDropped >= numToDrop)
                {
                    StopBombing();
                }
            }
        }


        Vector3.MoveTowards(transform.position, moveGoal, moveSpeed * Time.deltaTime);


    }

    private void Circle()
    {
        if (isCircling)
        {
            print(Quaternion.Angle(Quaternion.LookRotation(Vector3.forward, Vector3.up), Quaternion.LookRotation((transform.position - Vector3.zero).normalized, Vector3.up)));
        }
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
        player = GetClosestPlayer();

        //boming run first
        moveGoal = (player.transform.position + (Extns.xz3(player.transform.position) - Extns.xz3(transform.position)).normalized * bombDistance) + (Vector3.up * bombHeight);
        canAttack = false;
        shouldStartBombing = true;

    }


    private void StartBombing()
    {
        //anim play bombing anim
        
    }

    private void Bombing()
    {
        moveSpeed = bombingSpeed;
        moveGoal = (player.transform.position + (Extns.xz3(transform.position) - Extns.xz3(player.transform.position)).normalized * bombDistance) + (Vector3.up * bombHeight);

    }

    private void StopBombing()
    {
        shouldStartBombing = false;
        isBoming = false;
        canAttack = true;
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

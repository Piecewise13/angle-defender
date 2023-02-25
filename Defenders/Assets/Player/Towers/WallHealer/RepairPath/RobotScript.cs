using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RobotScript : MonoBehaviour
{

    private float speed;

    private float repairAmount;

    public NavMeshAgent agent;

    public float pauseDur;
    private float startPauseTime;

    private WallDefenceScript wall;
    private Transform returnPoint;
    private WallHealer_Script tower;

    private bool goingToWall;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance > 2f)
        {
            print("moving");
            startPauseTime = Time.time;
            return;
        }

        if (goingToWall)
        {
            print("At wall");
            agent.isStopped = true;
            if (startPauseTime + pauseDur < Time.time)
            {
                if (wall == null)
                {
                    tower.ReturnRobot(gameObject);
                    return;
                }
                wall.Repair(repairAmount);
                print("repair wall");
                agent.isStopped = false;
                agent.destination = returnPoint.position;
                goingToWall = false;
            }
        } else
        {
            if (agent.remainingDistance < 1f)
            {
                print("At Tower");
                agent.isStopped = false;
                tower.ReturnRobot(gameObject);
            }
        }
    }

    public void InitializeRobot(WallHealer_Script tower, WallDefenceScript target, Transform returnPoint)
    {
        agent.speed = speed;
        print(agent.SetDestination(target.gameObject.transform.position));
        
        wall = target;
        this.returnPoint = returnPoint;
        this.tower = tower;
        goingToWall = true;
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public void SetRepairAmount(float repairAmount)
    {
        this.repairAmount = repairAmount;
    }

}

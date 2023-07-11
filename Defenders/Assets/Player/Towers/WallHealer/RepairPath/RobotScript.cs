using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RobotScript : MonoBehaviour
{

    [SerializeField]private float speed;

    private float repairAmount;

    public NavMeshAgent agent;

    public float pauseDur;
    private float startPauseTime;

    private WallDefenceScript wall;
    private Transform returnPoint;
    private WallHealer_Script tower;

    private bool goingToWall;
    private bool atWall;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * STAGE ONE: ROBOT IS MOVING TOWARD THE WALL
         */
        if (goingToWall)
        {
            //ERROR HANDLING: Wait for agent's path to calculate
            if (!agent.hasPath)
            {
                return;
            }

            //agent is still going toward wall
            if (agent.remainingDistance > 2f)
            {
                return;
            }
            ArrivedAtWall();
        }


        /*
         * STAGE TWO: THE ROBOT IS AT THE WALL AND IS REPAIRING IT
         */
        if (atWall)
        {
            //pause for pauseDur
            if (startPauseTime + pauseDur > Time.time)
            {
                return;
            }

            //EDGE CASE: if the wall gets destroyed while robot is or on the way to heal the wall
            if (wall == null)
            {

            }

            RepairWall();
        }



        /*
         * STAGE THREE: ROBOT IS DONE REPAIRING THE WALL AND IS GOING BACK TO THE TOWER
         */

        //ERROR HANDLING: Wait for agent's path to calculate
        if (!agent.hasPath)
        {
            return;
        }

        //agent going back to tower
        if (agent.remainingDistance > 1f)
        {
            return;
        }

        ArrivedAtTower();
    }

    public void InitializeRobot(WallHealer_Script tower, WallDefenceScript target, Transform returnPoint)
    {
        agent.speed = speed;
        this.returnPoint = returnPoint;
        this.tower = tower;
        GoToWall(target);
    }

    private void GoToWall(WallDefenceScript targetWall)
    {
        wall = targetWall;
        agent.destination = targetWall.transform.position;
        goingToWall = true;
        atWall = false;
        print("GOING TO WALL");
    }

    private void ArrivedAtWall()
    {
        startPauseTime = Time.time;
        goingToWall = false;
        atWall = true;
        print("ARRIVED AT WALL");
    }

    private void RepairWall()
    {
        wall.Repair(repairAmount);
        GoBackToTower();
        print("REPAIRING WALL");
    }

    private void GoBackToTower()
    {
        atWall = false;
        agent.isStopped = false;
        agent.destination = tower.transform.position;
        print("GOING BACK TO TOWER");
    }
    
    private void ArrivedAtTower()
    {
        tower.ReturnRobot(gameObject);
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

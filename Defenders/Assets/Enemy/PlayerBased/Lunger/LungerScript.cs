using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LungerScript : PlayerBasedAIParent
{

    private float updatePlayerLocDelay;
    private float lastUpdatePlayerLoc;

    [Header("Lunge Vars")]
    [SerializeField] private float lungeDist;
    [SerializeField] private float maxLungeTime;
    [SerializeField] private float lungeForce;
    [SerializeField] private float maxHeight;


    public LayerMask wall;
    private Rigidbody rb;

    [SerializeField]private Transform playerTransform;

    private bool isLunge;
    
    // Start is called before the first frame update
    void Start()
    {
        
        player = ClosestPlayer(transform.position);
        
        //playerTransform = player.transform;
        rb = GetComponentInChildren<Rigidbody>();
        //initValues();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (updatePlayerLocDelay + lastUpdatePlayerLoc < Time.deltaTime)
        {
            UpdatePath();
        }
    }

    private void UpdatePath()
    {
        NavMeshPath path = new NavMeshPath();
        //print(agent);
        agent.CalculatePath(playerTransform.position, path);
        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, egg.transform.position, out hit, wall))
            {
                agent.destination = hit.transform.position;

            }
        } else
        {
            agent.destination = playerTransform.position;
        }
    }



    



    IEnumerator lungeAction(Vector3 loc)
    {
        agent.isStopped = true;
        float distance = Vector3.Distance(transform.position, loc);
        float timer = Mathf.Lerp(0f, maxLungeTime, distance / lungeDist);

        transform.LookAt(loc, Vector3.up);
        //anim start lunge
        yield return new WaitForSeconds(timer);
        //launch

        //Vector3 forceVector = transform.position - loc;
        float force = Mathf.Lerp(0, lungeForce, distance / lungeDist);
        //forceVector += Vector3.up  * -maxHeight;

        rb.AddForce(Vector3.up * maxHeight, ForceMode.Impulse);
        print("launched");

        yield return null;
    }

    public override void death()
    {
        throw new System.NotImplementedException();
    }

    public override void reachedEgg()
    {
        throw new System.NotImplementedException();
    }

    public override void takeDamage(float damage, Collider hitCollider)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerFound(PlayerScript player)
    {
        this.player = player;
        
        playerTransform = player.transform;
        StartCoroutine(lungeAction(playerTransform.position));
    }   
    
}

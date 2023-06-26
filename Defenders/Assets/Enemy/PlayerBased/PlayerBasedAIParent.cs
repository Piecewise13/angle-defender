using UnityEngine;
using UnityEngine.AI;

public abstract class PlayerBasedAIParent : ParentAIScript
{

    protected PlayerScript player;
    [SerializeField] protected float playerSearchRange;
    [SerializeField] protected LayerMask playerMask;


    protected static PlayerScript[] players;
    protected bool hasPlayer;

    public new void Start()
    {
        base.Start();
        players = FindObjectsOfType<PlayerScript>();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void PlayerFound(PlayerScript player);
    public abstract void PlayerLost(PlayerScript player);

    protected PlayerScript ClosestPlayer(Vector3 postion)
    {
        int closestIndex = -1;
        float currentDistance = float.MaxValue;
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isDead) {
                float distance = Vector3.Distance(postion, players[i].transform.position);
                if (distance < currentDistance && distance < playerSearchRange)
                {
                    closestIndex = i;
                }
            }
        }
        if (closestIndex == -1)
        {
            return null;
        }
        return players[closestIndex];
    }

    protected PlayerScript GetClosestPlayer()
    {
        int closestIndex = -1;
        float currentDistance = float.MaxValue;
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isDead)
            {
                float distance = Vector3.Distance(transform.position, players[i].transform.position);
                if (distance < currentDistance && distance < playerSearchRange)
                {
                    closestIndex = i;
                }
            }
        }
        if (closestIndex == -1)
        {
            return null;
        }
        return players[closestIndex];
    }

}

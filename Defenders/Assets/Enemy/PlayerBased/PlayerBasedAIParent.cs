using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBasedAIParent : ParentAIScript
{

    protected PlayerScript player;
    [SerializeField]protected float playerRange;
    [SerializeField] protected LayerMask playerMask;


    private static PlayerScript[] players;

    private void Awake()
    {
        initValues();
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
        int closestIndex = 0;
        float currentDistance = float.MaxValue;
        for (int i = 0; i < players.Length; i++)
        {
            float distance = Vector3.Distance(postion, players[i].transform.position);
            if (distance < currentDistance && distance < playerRange)
            {
                closestIndex = i;
            }
        }
        return players[closestIndex];
    }

}

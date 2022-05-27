using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBasedAIParent : ParentAIScript
{

    protected PlayerScript player;


    private static PlayerScript[] players;

    private void Awake()
    {
        players = FindObjectsOfType<PlayerScript>();

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void PlayerFound(PlayerScript player);

    protected PlayerScript ClosestPlayer(Vector3 postion)
    {
        int closestIndex = 0;
        float currentDistance = float.MaxValue;
        for (int i = 0; i < players.Length; i++)
        {
            if (Vector3.Distance(postion, players[i].transform.position) < currentDistance)
            {
                closestIndex = i;
            }
        }
        return players[closestIndex];
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAIScript : ParentAIScript
{
    private bool isSearching;

    public GameObject explosion;

    int explosionCount = 0;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        targetWall = GetWallTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetWall == null)
        {
            if (agent.hasPath)
            {
                isSearching = false;
                return;
            }
            isSearching = true;
            agent.SetDestination(egg.transform.position);
            return;
        }

        if (!agent.hasPath)
        {
            targetWall = GetWallTarget();
            if (targetWall == null)
            {
                GoToEgg();
                return;
            }
            agent.SetDestination(targetWall.transform.position);
        }

    }

    private void Explode()
    {
        if (explosionCount >= 1)
        {
            return;
        }
        explosionCount++;
        Instantiate(explosion, transform.position, transform.rotation);
        base.Death();
    }

    public override void AtWall(WallDefenceScript wall)
    {
        soulFireWorth = 0;
        base.AtWall(wall);
        Explode();

    }

    private WallDefenceScript GetWallTarget()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 2, egg.transform.position + Vector3.up * 2, out hit, LayerMask.NameToLayer("Defense")))
        {
            return hit.collider.gameObject.GetComponentInChildren<WallDefenceScript>();
        }

        return null;
    }

    public override void Death()
    {
        Explode();
    }

}

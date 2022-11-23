using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_EnemyDetection : MonoBehaviour
{
    private ParentAIScript enemy;
    private Collider enemyTrigger;
    private WallDefenceScript wallScript;

    private List<ParentAIScript> enemiesInTrigger = new List<ParentAIScript>();

    // Start is called before the first frame update
    void Start()
    {
        enemyTrigger = GetComponent<Collider>();
        wallScript = GetComponentInParent<WallDefenceScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Enemy"))
        {
            ParentAIScript script = other.transform.root.GetComponentInChildren<ParentAIScript>();
            script.AtWall(wallScript);
            enemiesInTrigger.Add(script);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Enemy"))
        {
            ParentAIScript script = other.transform.root.GetComponentInChildren<ParentAIScript>();
            script.LeaveWall();
            enemiesInTrigger.Remove(script);
        }
    }

    private void OnDisable()
    {
        foreach (var item in enemiesInTrigger)
        {
            item.LeaveWall();
        }
    }


}

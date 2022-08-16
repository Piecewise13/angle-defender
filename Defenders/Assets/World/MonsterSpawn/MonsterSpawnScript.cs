using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnScript : MonoBehaviour
{

    private int woodAmount;
    private int ironAmount;
    private int diamondAmount;

    [SerializeField] private int woodCost;
    [SerializeField] private int ironCost;
    [SerializeField] private int diamondCost;

    public ResourceType neededResource = ResourceType.Wood;




    BasicResourceCollector currentResourceCollector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        //print("enter");
        if (other.transform.root.gameObject.tag.Equals("Enemy"))
        {
            //print("checked");
            try
            {
                currentResourceCollector = other.gameObject.GetComponentInParent<BasicResourceCollector>();


                if (currentResourceCollector.collectedAmount > 0)
                {
                    UpdateResourceAmount(currentResourceCollector.collectedAmount, currentResourceCollector.collectedResource);
                    currentResourceCollector.collectedAmount = 0;
                    
                    //master.updateResourceAmount(enemyScript.collectedResource, enemyScript.collectedAmount);
                }

                currentResourceCollector.AssignTarget(neededResource);

            }
            catch
            {
                print("failed");
                return;
            }
        }
    }


    private void UpdateResourceAmount(int delta, ResourceType resource)
    {
        print("adding stuff");
        switch (resource)
        {

            case ResourceType.Wood:
                woodAmount += delta;
                if (woodAmount >= woodCost)
                {
                    neededResource = ResourceType.Iron;

                }
                break;
            case ResourceType.Iron:
                ironAmount += delta;

                if (ironAmount >= ironCost)
                {
                    neededResource = ResourceType.Diamond;
                }
                break;
            case ResourceType.Diamond:
                diamondAmount += delta;

                if (diamondAmount >= diamondCost)
                {
                    CheckIfSpawn();
                }
                break;
            case ResourceType.Count:
                break;
            default:
                break;
        }
    }

    private void CheckIfSpawn()
    {
        if (woodAmount < woodCost)
        {
            neededResource = ResourceType.Wood;
            return;
        }
        if (ironAmount < ironCost)
        {
            neededResource = ResourceType.Iron;
            return;
        }
        if (diamondAmount < diamondCost)
        {
            neededResource = ResourceType.Diamond;
            return;
        }
        Spawn();
    }

    public void Spawn()
    {
        Destroy(currentResourceCollector);
        print("Spawn");
    }

}

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


    private int soulFire;
    [SerializeField] private int soulFireNeeded;
    [SerializeField] private float tickDuration;
    private float lastTick;
    private bool bHasPlayer = false;
    [SerializeField] private int soulFireCost;
    public float shaderMax;
    public float shaderMin;

    public Renderer[] meter;


    [SerializeField] private List<GameObject> monsters = new List<GameObject>();
    public Transform spawnPoint;

    private PlayerScript player;

    public ResourceType neededResource = ResourceType.Wood;
    private bool spawned;



    BasicResourceCollector currentResourceCollector;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMeter();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawned)
        {
            return;
        }
        if (bHasPlayer)
        {
            if (tickDuration + lastTick < Time.time)
            {


                if (player.GetSoulFire() >= soulFireCost)
                {
                    soulFire += soulFireCost;
                    player.SetSoulFire(-soulFireCost);
                    UpdateMeter();
                    if (soulFire >= soulFireNeeded)
                    {
                        Spawn();
                    }
                    
                }
                lastTick = Time.time;
            }
        }   
    }
    public void OnTriggerEnter(Collider other)
    {
        //print("enter");
        if (other.transform.root.gameObject.tag.Equals("Enemy"))
        {
            ////print("checked");
            //try
            //{
            //    currentResourceCollector = other.gameObject.GetComponentInParent<BasicResourceCollector>();


            //    if (currentResourceCollector.collectedAmount > 0)
            //    {
            //        UpdateResourceAmount(currentResourceCollector.collectedAmount, currentResourceCollector.collectedResource);
            //        currentResourceCollector.collectedAmount = 0;
                    
            //        //master.updateResourceAmount(enemyScript.collectedResource, enemyScript.collectedAmount);
            //    }

            //    currentResourceCollector.AssignTarget(neededResource);

            //}
            //catch
            //{
            //    print("failed");
            //    return;
            //}
        } else if (other.transform.root.gameObject.tag.Equals("Player"))
        {
            player = other.GetComponentInParent<PlayerScript>();
            bHasPlayer = true;

        }


        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject.tag.Equals("Player"))
        {
            player = null;
            bHasPlayer = false;

        }
    }

    private void UpdateMeter()
    {

        for (int i = 0; i < meter.Length; i++)
        { 
            meter[i].material.SetFloat("_Origin", Mathf.Lerp(shaderMin, shaderMax, (float)soulFire / (float)soulFireNeeded));
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

        int index = Random.Range(0, monsters.Count);
        Instantiate(monsters[index], spawnPoint.position, spawnPoint.rotation);
        monsters.RemoveAt(index);

        spawned = true;
        Destroy(currentResourceCollector);
        print("Spawn");
    }

}

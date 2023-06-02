using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceSpawner : MonoBehaviour
{

    
    public List<DiamondNodeScript> resources = new List<DiamondNodeScript>();

    public int startingSpawnAmount;
    private int spawnAmount;

    private int count;

    public float range = 10.0f;

    public GameObject resourceNodePrefab;

    /*
    [Header("Resources")]
    public GameObject ironResource;
    public GameObject woodResource;
    public GameObject diamondResource;

    [Header("Resource Odds")]
    [SerializeField]
    private float woodChance;
    [SerializeField]
    private float ironChance;
    [SerializeField]
    private float diamondChance;
    */

    public NavMeshSurface navSurface;

    private void Start()
    {
        spawnAmount = startingSpawnAmount;
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result, out Vector3 normal)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {

                if (hit.mask != 8)
                {
                    
                    result = hit.position;
                    normal = hit.normal;
                    
                    return true;
                }
            }
        }

        result = Vector3.zero;
        normal = Vector3.zero;

        return false;
    }


    public void RemoveResource(DiamondNodeScript other)
    {
        resources.Remove(other);
        
    }


    public void SpawnResources()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            var script = ResourceNode();
            script.spawner = this;
            resources.Add(script);
        }
        count++;
        if (count % 5 == 0)
        {
            UpdateSpawnAmount();
        }
    }

    public void DespawnResources()
    {
        foreach (var item in resources)
        {
            if (item != null)
            {
                item.RemoveResource();
            }
        }
    }

    private DiamondNodeScript ResourceNode()
    {
        Vector3 randomPoint;
        Vector3 normal;

        while (!RandomPoint(transform.position, range, out randomPoint, out normal))
        {

        }

        return Instantiate(resourceNodePrefab, randomPoint, Quaternion.Euler(Vector3.up * Random.Range(0f, 180f))).GetComponent<DiamondNodeScript>();

    }

    private void UpdateSpawnAmount()
    {
        spawnAmount = startingSpawnAmount + (2 * (count / 5));
    }


    /*
    GameObject SpawnResource()
    {
        Vector3 randomPoint;
        Vector3 normal;

        while (!RandomPoint(transform.position, range, out randomPoint, out normal))
        {
            
        }


        float randomNum = Random.value;
        if (randomNum < diamondChance)
        {
            return Instantiate(diamondResource, randomPoint, Quaternion.Euler(Vector3.up * Random.Range(0f, 180f)));
            


        }
        else if (randomNum < ironChance)
        {
            return Instantiate(ironResource, randomPoint, Quaternion.Euler(Vector3.up * Random.Range(0f, 180f)));

        }
        else
        {
            
            return (Instantiate(woodResource, randomPoint, Quaternion.Euler(Vector3.up * Random.Range(0f, 180f))));
        }  
    }
    */

    /*
    public GameObject GetClostestResourceOfType(Vector3 refPos, ResourceType type)
    {
        GameObject currentClosest = null;
        float currentDistance = Mathf.Infinity;


        foreach (GameObject resource in resources)
        {

            ResourceScript resourceScript = resource.GetComponentInChildren<ResourceScript>();
            if (resourceScript.resource.Equals(type))
            {

                float distance = Vector3.Distance(refPos, resource.transform.position);
                if (currentDistance > distance)
                {

                    currentClosest = resource;
                    currentDistance = distance;
                }
            }
        }

        return currentClosest;

    }
    */
}

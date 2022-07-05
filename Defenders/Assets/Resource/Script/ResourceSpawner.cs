using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceSpawner : MonoBehaviour
{

    
    public ArrayList resources = new ArrayList();

    float timer;
    public float timeTilSpawn;

    public float startingSpawnAmount;

    public float range = 10.0f;


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


    public NavMeshSurface navSurface;
    

    private void Awake()
    {
        for (int i = 0; i < startingSpawnAmount; i++)
        {
            resources.Add(spawnResource());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeTilSpawn)
        {
            resources.Add(spawnResource());
            timer = 0f;
            
        }
        else
        {
            timer += Time.deltaTime;
        }
    }



    bool RandomPoint(Vector3 center, float range, out Vector3 result, out Vector3 normal)
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



    public void removeResource(GameObject other)
    {
        resources.Remove(other);
        
    }


    GameObject spawnResource()
    {
        Vector3 randomPoint;
        Vector3 normal;

        while (!RandomPoint(Vector3.zero, range, out randomPoint, out normal))
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour, Damageable
{

    public ResourceType resource;
    public GameObject spawnLocation;
    ResourceSpawner spawner;

    public float resourceHealth;
    
    public float health { get; set; }
    public bool isDead { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public Renderer model;

    public GameObject objectSpawn;

    // Start is called before the first frame update
    void Start()
    {
        health = resourceHealth;
        spawner = FindObjectOfType<ResourceSpawner>();
        
    }

    public void dropResource()
    {
        Instantiate(objectSpawn, spawnLocation.transform.position, spawnLocation.transform.rotation);
        
    }

    public void takeDamage(float damage, Collider hitCollider)
    {
        health -= damage;
        model.material.SetFloat("_CrackValue", Mathf.Lerp(1.5f, 0, health / resourceHealth));
        if (health <= 0)
        {
            death();

        }
    }

    public void death()
    {
        spawner.removeResource(gameObject);
        dropResource();
        Destroy(gameObject);
    }
}


public enum ResourceType
{
    Wood,
    Iron,
    Diamond
}
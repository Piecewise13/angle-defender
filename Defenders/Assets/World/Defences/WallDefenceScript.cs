using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallDefenceScript : MonoBehaviour, Damageable
{
    public ResourceType type;


    



    public float health { get; set; }
    public float maxHealth;
    public bool isDead { get; set; }

    [SerializeField]private GameObject wallMesh;
    public static NavMeshSurface navSurface;
    private NavMeshObstacle obstacle;
    

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        navSurface = FindObjectOfType<NavMeshSurface>();
        obstacle = GetComponentInParent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    



    public void takeDamage(float damage)
    {
        health -= damage;
    }

    public void takeDamage(float damage, Collider hitCollider)
    {
        if (!isDead)
        {
            health -= damage;
            //print(wallMesh.activeSelf);
            //print("Wall taking damage health: " + health);
            if (health <= 0.0f)
            {
                death();
            }
        }
    }

    public void death()
    {

        //print(wallMesh.activeSelf);
        wallMesh.SetActive(false);
        navSurface.BuildNavMesh();
        obstacle.enabled = false;
        isDead = true;
        
    }

    public void Rebuild()
    {
        wallMesh.SetActive(true);
        obstacle.enabled = true;
        isDead = false;
        navSurface.BuildNavMesh();
    }
}

public interface repairable
{
    float health { get; set; }
    float maxHealth { get; set; }
    ResourceType type { get; set; }
    float cost { get; set;}
    void repair();
}
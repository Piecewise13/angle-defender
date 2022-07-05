using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallDefenceScript : MonoBehaviour, Damageable
{
    public ResourceType type;

    public static Dictionary<ResourceType, int> cost = new Dictionary<ResourceType, int>();

    



    public float health { get; set; }
    public float maxHealth;
    public bool isDead { get; set; }

    [SerializeField]private GameObject wallMesh;
    private NavMeshObstacle obstacle;
    

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        obstacle = GetComponentInParent<NavMeshObstacle>();
        cost.Clear();
        cost.Add(ResourceType.Wood, 20);
        cost.Add(ResourceType.Iron, 0);
        cost.Add(ResourceType.Diamond, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    



    public void takeDamage(float damage)
    {
        health -= damage;
    }

    public void TakeDamage(float damage, Collider hitCollider)
    {
        if (!isDead)
        {
            health -= damage;
            //print(wallMesh.activeSelf);
            //print("Wall taking damage health: " + health);
            if (health <= 0.0f)
            {
                Death();
            }
        }
    }

    public void Death()
    {

        //print(wallMesh.activeSelf);
        wallMesh.SetActive(false);
        obstacle.enabled = false;
        isDead = true;
        
    }

    public void Rebuild()
    {
        wallMesh.SetActive(true);
        obstacle.enabled = true;
        isDead = false;
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
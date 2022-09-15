using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallDefenceScript : MonoBehaviour, Damageable
{
    public ResourceType type;

    public static Dictionary<ResourceType, int> cost = new Dictionary<ResourceType, int>();


    Material wallMat;
    public int lowMatVal;
    public int highMatVal;


    public GameObject[] wallObjects;
    private int currentWall;
    

    [SerializeField] public float health { get; set; }
    public float maxHealth;
    public bool isDead { get; set; }

    public GameObject wallHolder;
    [SerializeField]private GameObject wallObject;
    private NavMeshObstacle obstacle;
    public Collider collide;
    

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        obstacle = GetComponentInParent<NavMeshObstacle>();
        cost.Clear();
        cost.Add(ResourceType.Wood, 10);
        cost.Add(ResourceType.Iron, 0);
        cost.Add(ResourceType.Diamond, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    



    public void TakeDamage(float damage, Collider hitCollider)
    {
        if (!isDead)
        {
            health -= damage;
            ChangeWallObject();

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
        health = 0f;
        //print(wallMesh.activeSelf);
        //wallHolder.SetActive(false);
        collide.enabled = false;
        obstacle.enabled = false;
        isDead = true;
        
    }



    public void Rebuild()
    {
        collide.enabled = false;
        //wallHolder.SetActive(true);
        obstacle.enabled = true;
        isDead = false;
    }



    public void ChangeWallObject()
    {
        
        float healthPercentage = health / maxHealth;

        int index = Mathf.CeilToInt(Mathf.Lerp(wallObjects.Length - 1, 0, healthPercentage));
        //int index = Mathf.Clamp(Mathf.CeilToInt((1 - healthPercentage) * (wallObjects.Length)), 0, wallObjects.Length - 1);

        print("Health %: " + healthPercentage + ", index: " + index);

        if (currentWall != index)
        {
            Destroy(wallObject);
            print("trying to get: " + index);
            wallObject = Instantiate(wallObjects[index], wallHolder.transform);
            currentWall = index;
        }
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
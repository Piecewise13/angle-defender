using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour, Damageable
{

    private bool isOpen = false;
    private bool playerInTrigger = false;
    public PlayerScript player;
    public HUDScript[] playerHuds;
    public GameObject upgradeTree;
    public float maxHealth;
    private PlayerDataMangerScript playerData;


    public GameObject woodIngot;
    public GameObject ironIngot;
    public GameObject diamondIngot;
    public Transform ingotSpawnPoint;


   
    [SerializeField]private float resourceSpawnRate;
    private float woodLastSpawnTime;
    private float ironLastSpawnTime;
    private float diamondLastSpawnTime;

    private int upgradeNumber;

    public float health { get; set; }
    public bool isDead { get; set; }

// Start is called before the first frame update
void Start()
    {
        health = maxHealth;
        //print(health);
        playerHuds = FindObjectsOfType<HUDScript>();
        playerData = FindObjectOfType<PlayerDataMangerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetButtonDown("Use"))
            {
                spawnResources(ResourceType.Wood);
                print("Use Button");

                //playerScript.upgradeTreeOpen(!isOpen);
                isOpen = !isOpen;
                upgradeTree.SetActive(isOpen);
                player.openUIElement(isOpen);
                


            }
        }

        if (resourceSpawnRate + woodLastSpawnTime < Time.time)
        {
            spawnResources(ResourceType.Wood);
            woodLastSpawnTime = Time.time;
        }
        if (resourceSpawnRate * 3 + ironLastSpawnTime < Time.time)
        {
            spawnResources(ResourceType.Iron);
            ironLastSpawnTime = Time.time;
        }
        if (resourceSpawnRate * 10 + diamondLastSpawnTime < Time.time)
        {
            spawnResources(ResourceType.Diamond);
            diamondLastSpawnTime = Time.time;
        }
    }

    public void spawnResources(ResourceType type)
    {
        GameObject ingot = null;
        switch (type)
        {
            case ResourceType.Wood:
                ingot = Instantiate(woodIngot, ingotSpawnPoint.position, ingotSpawnPoint.rotation);
                break;
            case ResourceType.Iron:
                ingot = Instantiate(ironIngot, ingotSpawnPoint.position, ingotSpawnPoint.rotation);
                break;
            case ResourceType.Diamond:
                ingot = Instantiate(diamondIngot, ingotSpawnPoint.position, ingotSpawnPoint.rotation);
                break;
        }


        //Vector3.up
        ingot.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3,3), 10f, Random.Range(-3, 3)), ForceMode.Impulse);
    }

    public void UpgradeResourceRate(int upgradeNum)
    {
        //2(.85)^x for a final spawn rate of 4.29
        resourceSpawnRate -= (2 * Mathf.Pow(.85f, upgradeNum));

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            player = other.gameObject.GetComponentInParent<PlayerScript>();
            playerInTrigger = true;
        } else
        {
            ParentAIScript enemyScript = other.transform.root.gameObject.GetComponentInChildren<ParentAIScript>();

            TakeDamage(enemyScript.health / 2, null);
            enemyScript.ReachedEgg();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerInTrigger = false;
        }
    }


    public void TakeDamage(float damage, Collider hitCollider)
    {
        health -= damage;
        foreach (var item in playerHuds)
        {
            item.UpdateEggValues();
        }

        if (health <= 0f)
        {
            Death();
        }
    }

    public void Death()
    {
        playerData.GameLost();
    }
}

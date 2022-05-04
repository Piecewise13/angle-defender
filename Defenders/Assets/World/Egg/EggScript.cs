using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{

    private bool isOpen = false;
    private bool playerInTrigger = false;
    public PlayerScript player;
    public GameObject upgradeTree;

    private float eggHealth;
    public float maxHealth;

    public GameObject woodIngot;
    public GameObject ironIngot;
    public GameObject diamondIngot;
    public Transform ingotSpawnPoint;


   
    [SerializeField]private float resourceSpawnRate;
    private float woodLastSpawnTime;
    private float ironLastSpawnTime;
    private float diamondLastSpawnTime;

    private int upgradeNumber;


    // Start is called before the first frame update
    void Start()
    {
        eggHealth = maxHealth;

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
        ingot.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2,2), 10f, Random.Range(-2, 2)), ForceMode.Impulse);
    }

    public void UpgradeResourceRate()
    {
        upgradeNumber = 2;
        float adjustment = Mathf.Log(upgradeNumber + 1, 1.7f) - 1f;
        print(adjustment);
        resourceSpawnRate -= adjustment;


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            player = other.gameObject.GetComponentInParent<PlayerScript>();
            playerInTrigger = true;
        } else if (other.gameObject.tag.Equals("Enemy"))
        {
            ParentAIScript enemyScript = GetComponent<ParentAIScript>();
            enemyScript.reachedEgg();
            eggHealth -= enemyScript.health;
            if (eggHealth <= 0)
            {
                eggDeath(); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerInTrigger = false;
        }
    }


    private void eggDeath()
    {

    }

}

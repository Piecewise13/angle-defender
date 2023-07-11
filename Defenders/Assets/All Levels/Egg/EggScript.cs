using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour, Damageable
{

    private bool isOpen = false;
    private bool playerInTrigger = false;
    private PlayerScript player;
    public HUDScript[] playerHuds;
    public EggShopScript shop;
    public GameObject upgradeTree;
    public float maxHealth;
    private PlayerDataMangerScript playerData;


    public GameObject diamondIngot;
    public Transform ingotSpawnPoint;


   
    [SerializeField]private float resourceSpawnRate;
    private float lastResourceSpawnTime;

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

                //playerScript.upgradeTreeOpen(!isOpen);
                isOpen = !isOpen;
                upgradeTree.SetActive(isOpen);
                player.openUIElement(isOpen);
                if (isOpen)
                {
                    shop.SetPlayerInShop(player);
                } else
                {
                    shop.SetPlayerInShop(null);
                }


            }
        }

        if (resourceSpawnRate + lastResourceSpawnTime < Time.deltaTime)
        {
            SpawnResource();
            lastResourceSpawnTime = Time.time;
        }
    }

    public void SpawnResource()
    {
        GameObject ingot = Instantiate(diamondIngot, ingotSpawnPoint.position, ingotSpawnPoint.rotation);

        //Vector3.up
        ingot.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-4,4), 10f, Random.Range(-4, 4)), ForceMode.Impulse);
    }

    public void UpgradeResourceRate(int upgradeNum)
    {
        //2(.85)^x for a final spawn rate of 4.29
        resourceSpawnRate -= (2 * Mathf.Pow(.85f, upgradeNum));

    }


    private void OnTriggerEnter(Collider other)
    {

        string otherTag = other.transform.root.tag;
        if (otherTag.Equals("Player"))
        {
            player = other.gameObject.GetComponentInParent<PlayerScript>();
            playerInTrigger = true;
            player.hudScript.DisplayHint(PLAYER_HINT.USE);
        } else if (otherTag.Equals("Enemy"))
        {
            ParentAIScript enemyScript = other.transform.root.gameObject.GetComponentInChildren<ParentAIScript>();

            GiveDamage(enemyScript.health / 2);
            enemyScript.ReachedEgg();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerInTrigger = false;
            player.hudScript.StopDisplayingHint();
        }
    }

    public void Death()
    {
        playerData.GameLost();
    }

    public PlayerScript GetPlayer()
    {
        return player;
    }

    public void GiveDamage(float damage, Collider hitCollider, out float damageGiven, out bool crit)
    {
        damageGiven = 0;
        crit = false;
        GiveDamage(damage);
    }

    public void GiveDamage(float damage)
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
}

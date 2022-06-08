using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForge : MonoBehaviour
{
    private bool playerEnter;
    public PlayerScript player;
    public WeaponInventoryManager inventory;

    private int bulletsProduced;

    private int ironAmount = 10;
    public int ironCost;
    public int bulletAmount;

    public GameObject bulletForgeUI;

    private bool inMenu = false;

    [SerializeField] private float bulletProductionDelay;
    private float lastProductionTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ironAmount >= ironCost)
        {
            if (bulletProductionDelay + lastProductionTime < Time.time)
            {
                ProduceBullets();
                lastProductionTime = Time.time;
            }
        }

        if (playerEnter)
        {
            if (Input.GetButtonDown("Use"))
            {
                inMenu = !inMenu;
                bulletForgeUI.SetActive(inMenu);
                player.openUIElement(inMenu);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponentInParent<PlayerScript>();
            inventory = other.gameObject.GetComponentInParent<WeaponInventoryManager>();
            playerEnter = true;
            ReplenishBullets();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ReplenishBullets();
            playerEnter = false;
            
        }
    }


    public void ProduceBullets()
    {

            bulletsProduced += bulletAmount;
        
    }

    public void ReplenishBullets()
    {
        if (bulletsProduced > 0)
        {
            
            player.SetSoulFire(bulletsProduced);
            bulletsProduced = 0;
        }

    }

    public void ChangeIronAmount(int amount)
    {
        ironAmount += amount;
        inMenu = !inMenu;
        bulletForgeUI.SetActive(inMenu);
        player.openUIElement(inMenu);
        player.SetResourceAmount(ResourceType.Iron, -amount);
    }

    public void UpgradeForge(int upgradeAmount)
    {
        bulletAmount = Mathf.CeilToInt(bulletAmount * (1 + (upgradeAmount * .1f)));
    }
}

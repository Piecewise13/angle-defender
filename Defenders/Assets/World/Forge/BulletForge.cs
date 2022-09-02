using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForge : MonoBehaviour
{
    private bool playerEnter;
    public PlayerScript player;
    public WeaponInventoryManager inventory;

    private int playerBulletsProduced;
    private int turretBulletsProduced;

    private int playerWoodAmount = 0;
    private int turretWoodAmount;
    public int woodCost;
    public int bulletAmount;
    private bool hasWood;

    private bool inMenu = false;

    [SerializeField] private float bulletProductionDelay;
    private float lastProductionTime;

    //Gameobject Components
    public BulletForgeUI bulletForgeUI;
    public ParticleSystem[] eyeFire;
    public ParticleSystem mouthFire;

    [Space(20)]
    [Header("Soul Fire Line")]
    //SoulFire Line Stuff
    public GameObject[] soulFireLine;
    public Renderer[] lineMats;
    public Color activePink;
    public Color activeBlue;
    public Color disable;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasWood)
        {

            if (bulletProductionDelay + lastProductionTime < Time.time)
            {
                if (playerWoodAmount >= woodCost)
                {
                    ProducePlayerBullets();
                }
                if (turretWoodAmount >= woodCost)
                {
                    ProduceTurretBullets();
                }

                lastProductionTime = Time.time;
            }
        }

        if (playerEnter)
        {
            if (Input.GetButtonDown("Use"))
            {
                inMenu = !inMenu;
                bulletForgeUI.gameObject.SetActive(inMenu);
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


    public void ProducePlayerBullets()
    {

        playerBulletsProduced += bulletAmount;
        print("PlayerBullets: " + playerBulletsProduced);
        mouthFire.Play();

        playerWoodAmount += -woodCost;
        bulletForgeUI.UpdateWoodIndicator();
        if (playerWoodAmount < woodCost && turretWoodAmount < woodCost)
        {
            hasWood = false;
            StopEyeFire();
        }
    }
    public void ProduceTurretBullets()
    {

        TurretScript.bulletsReady += bulletAmount;
        turretWoodAmount += -woodCost;
        bulletForgeUI.UpdateWoodIndicator();
        if (turretWoodAmount < woodCost)
        {

            StopSoulFireTubes();
            if (playerWoodAmount < woodCost)
            {
                hasWood = false;
                StopEyeFire();
            }

        }
    }



    public void ReplenishBullets()
    {
        if (playerBulletsProduced > 0)
        {
            
            player.SetSoulFire(playerBulletsProduced);
            playerBulletsProduced = 0;
            mouthFire.Stop();
        }

    }

    public void ChangePlayerWoodAmount(int amount)
    {
        hasWood = true;
        StartEyeFire();

        playerWoodAmount += amount;

        player.SetResourceAmount(ResourceType.Wood, -amount);
    }
    
    
    public void ChangeTurretWoodAmount(int amount)
    {

        hasWood = true;
        StartEyeFire();
        turretWoodAmount += amount;
        StartSoulFireTubes();
        player.SetResourceAmount(ResourceType.Wood, -amount);
    }


    public void CloseMenu()
    {
        inMenu = false;
        bulletForgeUI.gameObject.SetActive(inMenu);
        player.openUIElement(inMenu);
    }

    public void OpenMenu()
    {
        inMenu = true;
        bulletForgeUI.gameObject.SetActive(inMenu);
        player.openUIElement(inMenu);
    }

    public void UpgradeForge(int upgradeAmount)
    {
        bulletAmount = Mathf.CeilToInt(bulletAmount * (1 + (upgradeAmount * .1f)));
    }

    public void TurretUnlocked()
    {
        for (int i = 0; i < soulFireLine.Length; i++)
        {
            soulFireLine[i].SetActive(true);
        }
        StopSoulFireTubes();
    }

    public int GetPlayerWood()
    {
        return playerWoodAmount;
    }

    public int GetTurretWood()
    {
        return turretWoodAmount;
    }



    private void StartEyeFire()
    {
        for (int i = 0; i < eyeFire.Length; i++)
        {
            eyeFire[i].Play();
        }
    }

    private void StartSoulFireTubes()
    {
        for (int i = 0; i < lineMats.Length; i++)
        {
            lineMats[i].material.SetColor("_FireBase", activePink);
            lineMats[i].material.SetColor("_FireSecondary", activeBlue);
            lineMats[i].material.SetFloat("_Speed", -.5f);
        }

    }

    private void StopSoulFireTubes()
    {
        for (int i = 0; i < lineMats.Length; i++)
        {
            lineMats[i].material.SetColor("_FireBase", disable);
            lineMats[i].material.SetColor("_FireSecondary", disable);
            lineMats[i].material.SetFloat("_Speed", 0f);
        }
    }

    private void StopEyeFire()
    {
        for (int i = 0; i < eyeFire.Length; i++)
        {
            eyeFire[i].Stop();
        }
    }



}

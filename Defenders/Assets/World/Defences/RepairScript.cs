using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairScript : MonoBehaviour
{

    WallDefenceScript wallDefence;
    private PlayerScript player;

    public float timeDelay;
    public float rebuildDelay;
    private float lastRepairTime;
    private float startRebuildTime;
    public int cost;
    public float repairAmount;


    // Start is called before the first frame update
    void Start()
    {
        wallDefence = GetComponentInParent<WallDefenceScript>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (Input.GetButtonDown("Use"))
            {
                lastRepairTime = Time.time;
                startRebuildTime = Time.time;

            }
            if (Input.GetButton("Use"))
            {
                if (wallDefence.health < wallDefence.maxHealth)
                {
                    player = other.GetComponentInParent<PlayerScript>();
                    repair();
                } 
            }
        }
    }

    

    public void repair()
    {
        if (lastRepairTime + timeDelay < Time.time)
        {

            if (CanAfford())
            {
                if (wallDefence.health <= 0)
                {
                    if (startRebuildTime + rebuildDelay < Time.time)
                    {
                        wallDefence.Rebuild();
                        wallDefence.health += repairAmount;
                        print(gameObject.name + " health: " + wallDefence.health);
                        ChargePlayer();
                    }

                } else
                {
                    if (wallDefence.health >= wallDefence.maxHealth)
                    {
                        wallDefence.health = wallDefence.maxHealth;
                        print("rebuilt all the way");
                    }
                    else
                    {
                        wallDefence.health += repairAmount;
                        print(gameObject.name + " health: " + wallDefence.health);
                        ChargePlayer();
                    }
                }


            }
            else
            {
                print("Can't Afford");
                //PLAY SOUND
            }
            lastRepairTime = Time.time;
        }
    }


    private bool CanAfford()
    {

        for (int i = 0; i < (int)ResourceType.Count; i++)
        {
            print(i + ", " + (ResourceType)i);
            if (player.GetResourceAmount((ResourceType)i) < WallDefenceScript.cost[(ResourceType)i])
            {
                
                return false;
            }
        }

        return true;
    }

    private void ChargePlayer()
    {
        for (int i = 0; i < (int)ResourceType.Count; i++)
        {
            player.SetResourceAmount((ResourceType)i, -WallDefenceScript.cost[(ResourceType)i]);
        }
    }
    
}

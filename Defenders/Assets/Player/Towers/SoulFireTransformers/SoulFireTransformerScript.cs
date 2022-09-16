using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFireTransformerScript : MonoBehaviour
{

    static BulletForge forge;
    public GameObject soulFireEffect;

    [SerializeField] int transferAmount;
    [SerializeField] private float transferTimer;
    private float startTransferTime;

    private bool isReady;
    private bool hasPlayer;

    PlayerScript player;

    // Start is called before the first frame update
    void Start()
    {
        forge = FindObjectOfType<BulletForge>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (hasPlayer) {
                if (startTransferTime + transferTimer < Time.time)
                {
                    if (forge.WithdrawFire(transferAmount))
                    {
                        startTransferTime = Time.time;
                        player.SetSoulFire(transferAmount);
                    } else
                    {
                        soulFireEffect.SetActive(false);
                    }

                }
            }
        }
    }

    public void Setup()
    {
        isReady = true;
        startTransferTime = Time.time;
        if (forge.fireStored >= transferAmount)
        {
            soulFireEffect.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            player = other.GetComponentInParent<PlayerScript>();
            hasPlayer = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            player = null;
            hasPlayer = false;
        }
    }

    public void ForgeHasFire(bool hasFire)
    {
        soulFireEffect.SetActive(hasFire);
        print(hasFire);
    }

}

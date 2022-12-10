using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseableParent : MonoBehaviour
{

    [SerializeField] private int cost;
    [SerializeField] private float purchaseTime = 2;
    private float timer;

    private bool isPurchased;

    public PlayerScript player;
    private bool hasPlayer;

    public GameObject purchaseObject;

    public PurchaseHUD hud;


    // Start is called before the first frame update
    void Start()
    {
        //hud = GetComponentInChildren<PurchaseHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPlayer)
        {
            if (Input.GetButton("Use"))
            {
                if (timer >= purchaseTime)
                {
                    if (player.CanAffordSoulFire(cost))
                    {

                        Purchase();
                    }
                    else
                    {
                        StartCoroutine(Extns.TextFlash(hud.costText));
                        timer = 0f;
                    }
                }
                else
                {
                    hud.UpdateFillAmount(timer / purchaseTime);
                    timer += Time.deltaTime;
                }

            }
        }
            if (Input.GetButtonUp("Use"))
            {
                hud.UpdateFillAmount(0f);
                timer = 0f;
            }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.CompareTag("Player"))
        {
            player = other.GetComponentInParent<PlayerScript>();
            hasPlayer = true;
            hud.gameObject.SetActive(true);
            hud.Setup(player);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject.CompareTag("Player"))
        {
            player = null;
            hasPlayer = false;
            hud.gameObject.SetActive(false);
        }
    }



    public virtual void Purchase()
    {
        player.SetSoulFire(-cost);
        isPurchased = true;
        purchaseObject.SetActive(true);
        Destroy(gameObject);
    }

    public int GetCost()
    {
        return cost;
    }

    public PlayerScript GetPlayer()
    {
        return player;
    }
}

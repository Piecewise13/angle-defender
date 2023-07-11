using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkBenchScript : MonoBehaviour
{
    [Header("Player Info")]
    public ModeManager playerInventory;
    public PlayerScript player;


    [Space(20)]
    [Header("UI Vars")]
    public GridLayoutGroup grid;
    public PurchaseTowerElementScript[] purchaseTowerObjects;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    [Space(10)]
    public LongClickButton_Slider longClickButton;

    [Space(10)]
    public TMP_Text playerWoodText;
    public TMP_Text playerIronText;
    public TMP_Text playerDiamondText;

    private int focusedTower;

    [SerializeField]private int unlockedTier = 1;


    public GameObject workbenchUI;
    private bool hasPlayer;
    private bool inMenu;
    private bool purchasedtower;


    // Start is called before the first frame update
    void Start()
    {
        workbenchUI.SetActive(false);
        UpdateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPlayer)
        {
            if (Input.GetButtonDown("Use"))
            {
                inMenu = !inMenu;
                player.openUIElement(inMenu);
                UpdatePlayerValues();
                workbenchUI.SetActive(inMenu);
                if (inMenu)
                {
                    purchasedtower = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            hasPlayer = true;
            player = other.GetComponentInParent<PlayerScript>();
            playerInventory = other.GetComponentInParent<ModeManager>();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            hasPlayer = false;
            player = null;
            playerInventory = null;
        }
    }


    public void PurchaseTowerSelected(PurchaseTowerElementScript tower)
    {
        for (int i = 0; i < purchaseTowerObjects.Length; i++)
        {
            if (purchaseTowerObjects[i].towerName.Equals(tower.towerName))
            {
                focusedTower = i;
            }

        }

        nameText.text = tower.towerName;
        descriptionText.text = tower.towerDescription;

        if (player.CanAffordResources(tower.diamondCost))
        {
            tower.longClickButton.canLongClick = true;
        } else
        {
            tower.longClickButton.canLongClick = false;
        }

    }


    public void GiveTower()
    {
        if (focusedTower == -1)
        {
            return;
        }

        PurchaseTowerElementScript tower = purchaseTowerObjects[focusedTower];
        /*
        if (playerInventory.GiveNewTower(tower.towerElements))
        {
            purchasedtower = true;
            player.SetResourceAmount(ResourceType.Wood, -tower.woodCost);
            player.SetResourceAmount(ResourceType.Iron, -tower.ironCost);
            player.SetResourceAmount(ResourceType.Diamond, -tower.diamondCost);
            UpdatePlayerValues();
            PurchaseTowerSelected(tower);
        } else
        {
            print("didn't work");
        }
        */
    }

    public void TierUnlocked(int tier)
    {

        this.unlockedTier = tier;
        UpdateGrid();
    }


    private void UpdateGrid()
    {
        foreach (var item in purchaseTowerObjects)
        {
            if (item.GetTier() <= unlockedTier)
            {
                item.gameObject.SetActive(true);

            } else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void UpdatePlayerValues()
    {

        playerDiamondText.text = "Current Diamond: " + player.GetDiamondAmount() + "";
    }

}

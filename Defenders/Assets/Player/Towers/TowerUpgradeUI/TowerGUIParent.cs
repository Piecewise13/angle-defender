using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TowerGUIParent : MonoBehaviour
{

    private Canvas canvas;
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    TowerParentScript towerParent;

    private TowerGUI_UpgradePath focusedUpgradePath;
    private PlayerScript player;


    public Button purchaseButton;
    private Image purchaseImage;
    public TMP_Text textPrompt;
    public TMP_Text purchaseText;
    public float purchaseOffset;


    public GameObject purchasePanel;
    private RectTransform purchasePanelRect;

    public TMP_Text upgradeName;
    public TMP_Text upgradeDesciption;

    public TMP_Text playerSoulFireText;

    public TMP_Text sellValueText;
    private int towerValue;
    private int sellValue;
    private static float sellMarkdown = .7f;

    protected int completeUpgradePaths;

    //TODO make it possible to max out the tower

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        towerParent = GetComponentInParent<TowerParentScript>();
        eventSystem = EventSystem.current;
        raycaster = GetComponent<GraphicRaycaster>();
        purchasePanel.SetActive(false);
        purchasePanelRect = purchasePanel.GetComponent<RectTransform>();
        SetTowerValue(towerParent.GetTowerCost()); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Set up the new Pointer Event
            pointerEventData = new PointerEventData(eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            pointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            raycaster.Raycast(pointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                try
                {
                    result.gameObject.GetComponent<TowerGUI_Interactable>().Interact();
                }
                catch { }
            }
        }
    }

    //Called when the purchase button is pressed
    public void PurchaseUpgrade()
    {
        if (focusedUpgradePath.CanBuyUpgrade())
        {
            SetTowerValue(focusedUpgradePath.GetCurrentCost());
            player.SetSoulFire(-focusedUpgradePath.GetCurrentCost());
            playerSoulFireText.text = player.GetSoulFire() + "";
            focusedUpgradePath.Upgrade();
        }
        
        
        

    }

    public void FocusUpgrade(TowerGUI_UpgradePath upgradePath)
    {
        
        if (focusedUpgradePath != null)
        {
            focusedUpgradePath.UnselectPath();
        }
        

        focusedUpgradePath = upgradePath;
        focusedUpgradePath.SelectPath();
        purchasePanel.SetActive(true);
        purchasePanelRect.position = focusedUpgradePath.GetComponent<RectTransform>().position + Vector3.right * purchaseOffset;
        print(player.CanAffordSoulFire(focusedUpgradePath.GetCurrentCost()));
        print(player.GetSoulFire());
        if (player.CanAffordSoulFire(focusedUpgradePath.GetCurrentCost()))
        {
            purchaseText.text = focusedUpgradePath.descriptionText;
            purchaseButton.interactable = true;
        } else
        {
            purchaseText.text = "CAN'T AFFORD";
            purchaseButton.interactable = false;
        }
        
    }

    public void UpgradePathComplete()
    {
        completeUpgradePaths++;
        if (completeUpgradePaths >= 3)
        {
            MaxOutTower();
        }
    }

    private void SetTowerValue(int delta)
    {
        towerValue += delta;
        sellValue = (int)(towerValue * sellMarkdown);
        sellValueText.text = sellValue + "";
        
    }

    public void SellTower()
    {
        player.SetSoulFire(sellValue);
        player.weaponManager.TowerRemoved(gameObject);
        towerParent.SwitchFromTowerCamera(player);
        player.weaponManager.TowerRemoved(towerParent.gameObject);
        Destroy(towerParent.gameObject);
    }

    protected virtual void MaxOutTower()
    {

    }

    public void ExitMenu()
    {
        towerParent.SwitchFromTowerCamera(player);
    }

    public static void SetSellMarkdown(float value)
    {
        sellMarkdown = value;
    }


    public void SetEventCamera(Camera playerCam)
    {
        //canvas.worldCamera = playerCam;
    }

    public void SetPlayer(PlayerScript player)
    {
        this.player = player;
    }

    public void OnEnable()
    {
        playerSoulFireText.text = player.GetSoulFire() + "";
    }
}

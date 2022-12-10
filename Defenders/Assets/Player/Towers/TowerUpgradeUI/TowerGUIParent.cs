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

    private TowerGUI_UpgradePath focusedUpgradePath;
    private PlayerScript player;
    public TowerGUI_Interactable purchaseButtonInteractable;
    private Button purchaseButton;

    public TMP_Text upgradeName;
    public TMP_Text upgradeDesciption;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        eventSystem = EventSystem.current;
        raycaster = GetComponent<GraphicRaycaster>();
        purchaseButton = purchaseButtonInteractable.GetComponent<Button>();
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

    public void PurchaseUpgrade()
    {
    
        if (player.CanAffordSoulFire(focusedUpgradePath.upgrades[focusedUpgradePath.currentUpgradeAvalible].cost))
        {
            player.SetSoulFire(-focusedUpgradePath.upgrades[focusedUpgradePath.currentUpgradeAvalible].cost);
            focusedUpgradePath.UpgradeBought(focusedUpgradePath.currentUpgradeAvalible);
        }

    }

    public void FocusUpgrade(TowerGUI_UpgradePath upgradePath)
    {
        if (focusedUpgradePath != null)
        {
            focusedUpgradePath.selectionImage.color = TowerGUI_UpgradePath.unselectedColor;
        }
        

        focusedUpgradePath = upgradePath;
        focusedUpgradePath.selectionImage.color = TowerGUI_UpgradePath.selectedColor;
        print(TowerGUI_UpgradePath.selectedColor);
        UpgradeInfo upgrade = focusedUpgradePath.upgrades[focusedUpgradePath.currentUpgradeAvalible];
        print(focusedUpgradePath.currentUpgradeAvalible);
        if (player.CanAffordSoulFire(upgrade.cost))
        {
            purchaseButtonInteractable.enabled = true;
            purchaseButton.interactable = true;
            upgradeName.text = upgrade.name + "";
            upgradeDesciption.text = upgrade.description + "";

        } else
        { 
            purchaseButtonInteractable.enabled = false;
            purchaseButton.interactable = true;
        }
        
    }


    public void SetEventCamera(Camera playerCam)
    {
        canvas.worldCamera = playerCam;
    }

    public void SetPlayer(PlayerScript player)
    {
        this.player = player;
    }
}

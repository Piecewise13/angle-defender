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
    private Image purchaseImage;
    public TMP_Text textPrompt;
    public TMP_Text purchaseText;
    public float purchaseOffset;


    public GameObject purchasePanel;
    private RectTransform purchasePanelRect;

    public TMP_Text upgradeName;
    public TMP_Text upgradeDesciption;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        eventSystem = EventSystem.current;
        raycaster = GetComponent<GraphicRaycaster>();
        purchaseButton = purchaseButtonInteractable.GetComponent<Button>();
        purchasePanel.SetActive(false);
        purchasePanelRect = purchasePanel.GetComponent<RectTransform>();
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
        print("purchasing");
        player.SetSoulFire(-focusedUpgradePath.GetCurrentCost());
        focusedUpgradePath.UpgradeBought();

    }

    public void FocusUpgrade(TowerGUI_UpgradePath upgradePath)
    {
        
        //print("called");
        if (focusedUpgradePath != null)
        {
            focusedUpgradePath.UnselectPath();
        }
        

        focusedUpgradePath = upgradePath;
        focusedUpgradePath.SelectPath();

        if (player.CanAffordSoulFire(focusedUpgradePath.GetCurrentCost()))
        {
            //RectTransform rectTransform = upgradePath.GetComponent<RectTransform>();
            purchasePanel.SetActive(true);
            purchasePanel.transform.position = upgradePath.transform.position + (transform.right * purchaseOffset * -1);
            //purchaseButtonInteractable.enabled = true;
            //purchaseButton.interactable = true;


        } else
        {
            purchasePanel.SetActive(false);
            //purchaseButtonInteractable.enabled = false;
            //purchaseButton.interactable = true;
        }
        
    }


    public void SetEventCamera(Camera playerCam)
    {
        //canvas.worldCamera = playerCam;
    }

    public void SetPlayer(PlayerScript player)
    {
        this.player = player;
    }
}

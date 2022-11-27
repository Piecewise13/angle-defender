using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerGUIParent : MonoBehaviour
{

    private Canvas canvas;
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        eventSystem = EventSystem.current;
        raycaster = GetComponent<GraphicRaycaster>();
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

    public void Test()
    {
        print("button works");
    }

    public void SetEventCamera(Camera playerCam)
    {
        canvas.worldCamera = playerCam;
    }
}

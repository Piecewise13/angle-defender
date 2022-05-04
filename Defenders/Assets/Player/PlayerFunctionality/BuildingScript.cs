using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{

    public GameObject[] blockMarkerArray;
    public GameObject[] blockArray;

    private int blockIndex;

    private bool inBuildMode = false;
    private bool resetValidate = true;

    private GameObject blockMarker;
    public float markerRotateSpeed;
    public LayerMask buildingLayers;
    private string buildingTag;
    bool validPlacement;

    public int buildingCost;
    public float upgradeCost;

    private bool isUpgrading = false;

    private Camera playerCamera;
    private PlayerScript playerScript;
    public GameObject weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerScript = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Build Key"))
        {
            if (inBuildMode)
            {


                setBuildMode(false);
            }
            else
            {
                setBuildMode(true);
            }

        }

        if (inBuildMode)
        {

            if (Input.GetKeyDown("1"))
            {
                //floor
                Destroy(blockMarker);
                blockIndex = 0;
                buildingTag = "Floor";
                blockMarker = Instantiate(blockMarkerArray[blockIndex]);
                buildingLayers = LayerMask.GetMask(new string[] { "Block", "Ground", "FloorCollider" });
                resetValidate = true;


            }
            else if (Input.GetKeyDown("2"))
            {
                //wall
                Destroy(blockMarker);
                buildingLayers = LayerMask.GetMask(new string[] { "Block", "Ground", "WallCollider" });
                blockIndex = 1;
                buildingTag = "Wall";
                blockMarker = Instantiate(blockMarkerArray[blockIndex]);
                resetValidate = true;


            }
            else if (Input.GetKeyDown("3"))
            {

                blockIndex = 2;
                blockMarker = Instantiate(blockMarkerArray[blockIndex]);
            }
            else if (Input.GetKeyDown("4"))
            {
                blockIndex = 3;
                blockMarker = Instantiate(blockMarkerArray[blockIndex]);
            }
            RaycastHit hit;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, 10, buildingLayers))
            {

                if (hit.collider)
                {
                    if (!isUpgrading) {
                        if (playerScript.getResourceAmount(ResourceType.Wood) >= buildingCost) {
                            if (hit.collider.tag.Equals(buildingTag))
                            {

                                if (!validPlacement || resetValidate)
                                {
                                    resetValidate = false;
                                    validPlacement = true;
                                    blockMarker.SendMessage("validMaterial");

                                }



                                blockMarker.transform.position = hit.collider.transform.position;
                                blockMarker.transform.rotation = hit.collider.transform.rotation;
                                if (Input.GetButtonDown("Fire1") && validPlacement)
                                {
                                    Instantiate(blockArray[blockIndex], blockMarker.transform.position, blockMarker.transform.rotation);
                                    playerScript.updateResourceAmount(ResourceType.Wood, -buildingCost);
                                    hit.collider.gameObject.SetActive(false);
                                }

                            }
                            else
                            {
                                if (validPlacement || resetValidate)
                                {
                                    validPlacement = false;
                                    resetValidate = false;
                                    blockMarker.SendMessage("invalidMaterial");

                                }

                                blockMarker.transform.position = hit.point;

                                float rotateValue = Input.GetAxis("Rotate Block");

                                Vector3 rotateVector = rotateValue * Vector3.up;

                                blockMarker.transform.Rotate(rotateVector * markerRotateSpeed * Time.deltaTime);

                            }
                        } else
                        {
                            setBuildMode(false);
                            print("Not Enough Building Mats");
                        }
                    }
                }
            }
        }
    }

    public void setBuildMode(bool value)
    {
        
        if (value)
        {
            inBuildMode = true;
            blockMarker = Instantiate(blockMarkerArray[blockIndex]);
            resetValidate = true;
            weaponManager.SetActive(false);

        }
        else
        {
            print("exit");
            inBuildMode = false;
            Destroy(blockMarker);
            resetValidate = false;
            weaponManager.SetActive(true);

        }
    }

    public bool getBuildMode()
    {
        return inBuildMode;
    }
}

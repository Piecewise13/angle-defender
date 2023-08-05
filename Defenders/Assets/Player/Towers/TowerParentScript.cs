using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class TowerParentScript : MonoBehaviour
{

    protected PlayerScript player;

    [SerializeField] protected TowerGUIParent towerUI;
    [SerializeField] protected float uiDistanceFromTower;

    protected Renderer[] meshes;

    protected Material[] defaultMaterials;

    protected bool isPlaced;
    public LayerMask placementLayers;

    [SerializeField]protected bool isSnapping;

    protected Camera towerCamera;

    [SerializeField] private int towerCost;

    BoxCollider towerCollider;



    // Start is called before the first frame update
    public void Awake()
    {
        towerUI.gameObject.SetActive(false);

        towerCamera = GetComponentInChildren<Camera>();
        towerCamera.gameObject.SetActive(false);

        towerCollider = GetComponent<BoxCollider>();
        towerCollider.enabled = false;

        meshes = GetComponentsInChildren<Renderer>();
        //print(meshes.Length);
        defaultMaterials = new Material[meshes.Length];
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i] = meshes[i].material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlaced)
        {
            return;
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject.tag.Equals("Player"))
        {
            towerUI.gameObject.SetActive(false);
        }

    }
    */

    public void SetMaterials(Material mat)
    {
        //print(meshes.Length);
        foreach (Renderer render in meshes)
        {
            render.material = mat;
        }
    }

    private void SetMaterialsToDefault()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = defaultMaterials[i];
        }
    }

    public void SwitchToTowerCamera(PlayerScript player)
    {
        SetPlayer(player);
        Cursor.lockState = CursorLockMode.Confined;
        towerCamera.gameObject.SetActive(true);
        player.playerCamera.gameObject.SetActive(false);
        player.OpenMenu(true);
        print("Tower menu on");
        towerUI.gameObject.SetActive(true);        
    }

    public void SwitchFromTowerCamera(PlayerScript player)
    {
        Cursor.lockState = CursorLockMode.Locked;
        player.playerCamera.gameObject.SetActive(true);
        towerCamera.gameObject.SetActive(false);
        towerUI.gameObject.SetActive(false);
        player.OpenMenu(false);
    }

    protected List<Transform> FindSpawnPoints(GameObject parent)
    {
        List<Transform> result = new List<Transform>();
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag("SpawnPoint"))
            {
                result.Add(child);
            }
            //print(child.gameObject);
            result.AddRange(FindSpawnPoints(child.gameObject));
        }

        return result;

    }



    /*
     * VIRTUAL METHODS
     */
    public virtual void Place()
    {
        isPlaced = true;
        SetMaterialsToDefault();
        towerCollider.enabled = true;

    }

    public virtual void RemoveTower()
    {

    }

    public void SetTowerCost(int cost)
    {
        towerCost = cost;
        
    }

    public int GetTowerCost()
    {
        return towerCost;
    }

    public virtual PlayerScript GetPlayer()
    {
        return player;
    }

    public virtual void SetPlayer(PlayerScript value)
    {
        player = value;
        towerUI.SetPlayer(player);
    }

    public bool GetIsSnapping()
    {
        return isSnapping;
    }
}

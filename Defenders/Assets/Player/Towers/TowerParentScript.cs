using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TowerParentScript : MonoBehaviour
{

    protected PlayerScript player;

    [SerializeField] protected TowerGUIParent towerUI;
    [SerializeField] protected float uiDistanceFromTower;

    protected Renderer[] meshes;

    protected Material[] defaultMaterials;

    protected bool isPlaced;

    protected Camera towerCamera;


    // Start is called before the first frame update
    public void Awake()
    {
        towerUI.gameObject.SetActive(false);

        towerCamera = GetComponentInChildren<Camera>();
        towerCamera.gameObject.SetActive(false);

        meshes = GetComponentsInChildren<Renderer>();
        //print(meshes.Length);
        defaultMaterials = new Material[meshes.Length];
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i] = meshes[i].material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlaced)
        {
            return;
        }
        if (other.transform.root.gameObject.tag.Equals("Player"))
        {
            player = other.GetComponent<PlayerScript>();
            SetPlayer(player);
            towerUI.SetEventCamera(player.playerCamera);
            Vector3 playerOffset = player.transform.position + player.transform.forward * 5;
            Vector3 towerOffset = transform.position - player.transform.position;
            //Debug.DrawRay(player.transform.position, towerOffset, Color.red);
            //print(towerOffset);
            //print(Vector3.Angle(towerOffset, playerOffset));
            Vector3 uiLocation = transform.position + Vector3.Cross(towerOffset, Vector3.up).normalized * uiDistanceFromTower;
            uiLocation = Extns.xz3(player.transform.position + (uiLocation - player.transform.position).normalized * 10) + Vector3.up * 3; 
            //print(uiLocation);
            towerUI.transform.position = uiLocation;
            towerUI.transform.LookAt(player.transform.position, Vector3.up);
            towerUI.transform.Rotate(Vector3.up, 180);

            towerUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject.tag.Equals("Player"))
        {
            towerUI.gameObject.SetActive(false);
        }

    }

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
        Cursor.lockState = CursorLockMode.Confined;
        towerCamera.gameObject.SetActive(true);
        player.playerCamera.gameObject.SetActive(false);
        towerUI.gameObject.SetActive(true);        
    }

    public void SwitchFromTowerCamera(PlayerScript player)
    {
        Cursor.lockState = CursorLockMode.Locked;
        player.playerCamera.gameObject.SetActive(true);
        towerCamera.gameObject.SetActive(false);
        towerUI.gameObject.SetActive(false);
    }



    /*
     * VIRTUAL METHODS
     */
    public virtual void Place()
    {
        isPlaced = true;
        SetMaterialsToDefault();
    }

    public virtual void RemoveTower()
    {

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
}

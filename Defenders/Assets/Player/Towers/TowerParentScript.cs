using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class TowerParentScript : MonoBehaviour
{

    protected PlayerScript player;

    [SerializeField] protected TowerGUIParent towerUI;
    [SerializeField] protected float uiDistanceFromTower;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //NEXT TODO
    //GIVE EACH BUTTON DAMAGABLE SCRIPT AND THEN TRIGGER AN EVENT FOR EACH ONE USING THE INTERFACE ON THE LONG CLICK SCRIPT 
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag.Equals("Player"))
        {
            player = other.GetComponent<PlayerScript>();
            towerUI.SetEventCamera(player.playerCamera);
            Vector3 playerOffset = player.transform.position + player.transform.forward * 5;
            Vector3 towerOffset = transform.position - player.transform.position;
            //Debug.DrawRay(player.transform.position, towerOffset, Color.red);
            print(towerOffset);
            //print(Vector3.Angle(towerOffset, playerOffset));
            Vector3 uiLocation = transform.position + Vector3.up * 2 + Vector3.Cross(towerOffset, Vector3.up).normalized * uiDistanceFromTower;
            uiLocation = player.transform.position + (uiLocation - player.transform.position).normalized * 10; 
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




    /*
     * VIRTUAL METHODS
     */
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

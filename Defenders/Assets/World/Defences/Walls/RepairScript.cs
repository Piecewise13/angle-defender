using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairScript : MonoBehaviour
{

    WallDefenceScript wallDefence;
    private PlayerScript player;

    public float timeDelay;
    public float rebuildDelay;
    private float lastRepairTime;
    private float startRebuildTime;
    public int cost;
    public float repairAmount;


    // Start is called before the first frame update
    void Start()
    {
        wallDefence = GetComponentInParent<WallDefenceScript>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            wallDefence.PlayerEnter(other.GetComponentInParent<PlayerScript>());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            wallDefence.PlayerExit();
        }
    }









    
}

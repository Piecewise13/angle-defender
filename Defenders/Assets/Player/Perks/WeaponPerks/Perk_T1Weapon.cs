using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_T1Weapon : ParentPerkScript
{

    public GameObject armory;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UnlockUpgrade()
    {
        armory.SetActive(true);
        
    }
}

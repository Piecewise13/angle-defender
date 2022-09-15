using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableTowerScript : MonoBehaviour
{
    public string towerName;

    public GameObject towerThrowable;

    public WeaponInventoryManager playerInventory;
    public bool canFire;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled && canFire)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (playerInventory.TowerThrown())
                {
                    {
                        Instantiate(towerThrowable, transform.position, transform.rotation);
                    }

                }
            }
        }



    }
}

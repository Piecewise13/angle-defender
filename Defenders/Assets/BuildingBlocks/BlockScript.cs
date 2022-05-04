using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{

    Collider colliderThis;
    public GameObject Marker;
    private LayerMask layers;
    

    // Start is called before the first frame update
    void Start()
    {
        
        colliderThis = GetComponent<Collider>();
        layers = LayerMask.GetMask("Block");
        if (Physics.CheckBox(gameObject.transform.TransformVector(gameObject.transform.position), Vector3.one * .25f, Quaternion.Euler(0f, 0f, 0f), layers))
        {
            colliderThis.enabled = false;
            print(colliderThis.name);
        }
        
        //foreach (Collider collide in Physics.OverlapBox(gameObject.transform.TransformVector(gameObject.transform.position), Vector3.one * .3f, Quaternion.Euler(0f,0f,0f),8))

        //{

        //    colliderThis.enabled = false;
            
        //    print(collide.name);

        //}

    }


}

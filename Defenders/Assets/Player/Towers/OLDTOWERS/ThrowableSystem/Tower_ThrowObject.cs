using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_ThrowObject : MonoBehaviour
{

    private Rigidbody rb;
    static float force = 20;
    public LayerMask layersToHit;
    public GameObject tower;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        print("object spawned");
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.transform.root.tag;
        print(collision.GetContact(0) + " tag: " + tag);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f, layersToHit))
        {

            Instantiate(tower, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal));
            print(tower);
            Destroy(gameObject);
        } else
        {
            Instantiate(tower, transform.position, Quaternion.FromToRotation(Vector3.up, collision.GetContact(0).normal));
        }
        

    }
}

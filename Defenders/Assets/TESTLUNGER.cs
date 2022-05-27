using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTLUNGER : MonoBehaviour
{

    public Transform target;

    public Rigidbody rb;

    public float force;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 launchVector = target.position - transform.position;

        rb.AddForce(launchVector + (transform.up * force), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

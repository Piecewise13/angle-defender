using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingRocketScript : MonoBehaviour
{
    public int numBounces;

    [SerializeField] public float launchVelo;

    private Rigidbody rb;

    private Vector3 impactVector;

    private Vector3 currentVelo;

    bool startGravity = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * launchVelo, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        if (startGravity)
        {
            currentVelo.y += -10f * Time.deltaTime;

        }
        transform.Translate(currentVelo * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //explode or create what ever effect
        startGravity = true;
        if (numBounces > 5)
        {
            Destroy(gameObject);
        }

        //rb.AddForce(CalcNewLaunchVector(collision.GetContact(0).normal), ForceMode.Impulse);
        CalcNewLaunchVector(collision.GetContact(0).normal);
        //print(launchVelo);
    }


    private void CalcNewLaunchVector(Vector3 surfaceNormal)
    {
        numBounces++;
        Vector3 reflectVector = Vector3.Reflect(impactVector, surfaceNormal).normalized;
        currentVelo = reflectVector * launchVelo;
        launchVelo /= 8;
    }

    public void SetDestination(Vector3 destination)
    {
        transform.LookAt(destination);
        impactVector = transform.forward;
        currentVelo = transform.forward * launchVelo;
        launchVelo /= 4;
    }
}

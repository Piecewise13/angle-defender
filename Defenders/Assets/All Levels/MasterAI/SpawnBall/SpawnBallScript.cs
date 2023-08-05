using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBallScript : MonoBehaviour
{

    public GameObject enemy;

    private float time;
    public float force = 5;
    private float angle = 60f;

    const float GRAVITY = -9.89f;
    Vector3 initPos;

    private Rigidbody rb;
    public Parent_MasterAI masterAI;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        force = Random.Range(5f, 10f);
        rb = GetComponent<Rigidbody>();
        Vector3 forceDir = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * force + Vector3.up * force/2;
        rb.AddForce(forceDir, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = initPos +  transform.forward * x_val() + transform.up * y_val();
        //time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //
    }

    private void OnCollisionEnter(Collision collision)
    {
        masterAI.ai.Add(Instantiate(enemy, collision.contacts[0].point, Quaternion.Euler(Vector3.zero)));
        Destroy(gameObject);
    }



    //private float x_val()
    //{
    //    float vi = force * Mathf.Cos(Mathf.Deg2Rad * angle);
    //    float value = vi * time;
    //    return value;
    //}

    //private float y_val()
    //{
    //    float vi = force * Mathf.Sin(Mathf.Deg2Rad * angle);
    //    float value = ((GRAVITY / 2) * time * time) + (vi * time);
    //    return value;
    //}




}

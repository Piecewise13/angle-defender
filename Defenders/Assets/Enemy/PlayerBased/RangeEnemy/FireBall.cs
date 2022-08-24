using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    
    public Transform target;


    private Vector3 initPos;
    float time;

    float initVelo;
    float distance;
    float angle = Mathf.Deg2Rad * 50f;
    const float gravity = 9.81f;


    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        distance = Vector3.Distance((transform.position), (target.position + ((transform.position - target.position).normalized) * initPos.y ));
        initVelo = Mathf.Sqrt((gravity * distance)/(Mathf.Sin(2 * angle)));
        print(initVelo);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initPos + (transform.forward * initVelo * time * Mathf.Cos(angle)) + (transform.up * ((initVelo * time * Mathf.Sin(angle)) - (.5f * gravity * time * time)));
        time += Time.deltaTime;
    }
}

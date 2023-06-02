using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    
    [HideInInspector]public Transform target;
    public GameObject explosion;
    public float radius;
    public float damage;
    public LayerMask layerToHit;

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


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initPos + (transform.forward * initVelo * time * Mathf.Cos(angle)) + (transform.up * ((initVelo * time * Mathf.Sin(angle)) - (.5f * gravity * time * time)));
        time += Time.deltaTime;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerToHit);
        foreach (var item in hits)
        {
            try
            {
                item.GetComponentInParent<Damageable>().GiveDamage(damage);
                print(item);
            }
            catch { }
        }

        Instantiate(explosion, transform.position, Quaternion.Euler(Vector3.zero));
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{

    public float speed;
    public Vector3 destination;
    public float radius;
    public LayerMask layerToHit;
    public float damage;
    public float freezeTime;
    public GameObject freezeDecal;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(destination);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerToHit);
        foreach (var item in hits)
        {
            try
            {
                ParentAIScript script = item.GetComponentInParent<ParentAIScript>();
                script.TakeDamage(damage, item);
                script.Freeze(freezeTime);
                
            }
            catch { }
        }
        //Instantiate(explosion, transform.position, Quaternion.Euler(Vector3.zero));
        //Instantiate(freezeDecal, transform.position + Vector3.up * 5, Quaternion.Euler(Vector3.zero));
        //Destroy(gameObject);
    }
}

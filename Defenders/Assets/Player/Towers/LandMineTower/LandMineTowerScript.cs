using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineTowerScript : MonoBehaviour
{

    public GameObject explosion;
    [SerializeField] private float radius;
    [SerializeField] private float damage;
    public LayerMask layer;



    private void OnTriggerEnter(Collider other)
    {
        print("Collider enter");
        if (other.transform.root.tag.Equals("Enemy"))
        {
            print("has enemy");
            Explode();
        }
    }

    public void Explode()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, radius, layer);
        foreach (var item in enemies)
        {
            ParentAIScript script = item.GetComponentInParent<ParentAIScript>();
            script.TakeDamage(damage, item);
        }
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}

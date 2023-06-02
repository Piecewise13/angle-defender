using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher_BasicRocket : RocketScript
{
    [SerializeField] float radius;
    public GameObject explosion;
    [SerializeField] private float damage;

    public LayerMask layerToHit;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerToHit);
        if (hits.Length != 0)
        {
            foreach (var item in hits)
            {
                ParentAIScript script = item.GetComponentInParent<ParentAIScript>();
                //TODO MAKE IT SO THAT A DAMAGE MARKER SPAWNS ON DAMAGE
                script.GiveDamage(damage);
            }
        }


        Instantiate(explosion, transform.position, Quaternion.Euler(Vector3.zero));
        Destroy(gameObject);
    }
}

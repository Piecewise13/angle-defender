using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher_SplitBomb : MonoBehaviour
{
    private Vector3 moveDir;

    public Rigidbody rb;

    [SerializeField] private float force;

    public GameObject explosion;
    [SerializeField] private float radius;
    public LayerMask layerToHit;
    [SerializeField] private float damage;

    private void Start()
    {
        moveDir = transform.forward + transform.up;
        rb.AddForce(moveDir * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Enemy"))
        {
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerToHit);
        if (hits.Length != 0)
        {
            foreach (var item in hits)
            {
                ParentAIScript script = item.GetComponentInParent<ParentAIScript>();
                script.TakeDamage(damage, item);
            }
        }

        Instantiate(explosion, transform.position, Quaternion.Euler(Vector3.zero));
        Destroy(gameObject);
    }
}

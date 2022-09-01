using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDemonBomb : MonoBehaviour
{

    public GameObject explosion;
    public float radius;
    [SerializeField] private float damage;

    public float bombSpeed;
    public LayerMask layerToHit;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.AddForce(Vector3.down * bombSpeed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.root.tag.Equals("Monster"))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerToHit);
            foreach (var item in hits)
            {
                try
                {
                    item.GetComponentInParent<Damageable>().TakeDamage(damage, item);
                } catch { }
            }
            Instantiate(explosion, transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(gameObject);
        }
    }
}

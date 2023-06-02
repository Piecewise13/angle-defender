using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber_ExplosionScript : MonoBehaviour
{

    [SerializeField] private float range;
    [SerializeField] private float damage;

    public LayerMask layersToHit;

    // Start is called before the first frame update
    void Start()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, range, layersToHit);
        foreach (var obj in hit)
        {
            if (obj.transform.root.CompareTag("Player"))
            {
                if (Physics.Linecast(transform.position + Vector3.up * 2, obj.transform.position, LayerMask.GetMask("Defense")))
                {
                    continue;
                }
            }

            Damageable damageScript = obj.transform.root.GetComponentInChildren<Damageable>();
            damageScript.GiveDamage(damage);
            Destroy(gameObject);
        }
    }
}

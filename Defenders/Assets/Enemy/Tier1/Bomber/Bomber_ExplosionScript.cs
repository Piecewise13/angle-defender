using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber_ExplosionScript : MonoBehaviour
{

    [SerializeField] private float range;
    [SerializeField] private float damage;

    public LayerMask layersToHit;

    bool canDamagePlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        //TODO WILL NOT WORK WITH MULTIPLAYER
        Collider[] hit = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Player"));
        foreach (var obj in hit)
        {
            print("Player in range");
            if (Physics.Linecast(transform.position + Vector3.up * 2, obj.transform.position, LayerMask.GetMask("Defense")))
            {
                canDamagePlayer = false;
            }
        }

        hit = Physics.OverlapSphere(transform.position, range, layersToHit);
        foreach (var obj in hit)
        {
            if (obj.transform.root.CompareTag("Player"))
            {
                if (!canDamagePlayer)
                {
                    continue;
                }
            }

            Damageable damageScript = obj.transform.root.GetComponentInChildren<Damageable>();
            damageScript.GiveDamage(damage);
            
        }

        Invoke("Remove", 5f);
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}

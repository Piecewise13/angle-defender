using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class IngotScript : MonoBehaviour
{
    public ResourceType type;
    public int amount;
    private static bool upgraded = false;

    SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (upgraded)
        {
            sphereCollider.radius = 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.transform.root.gameObject.tag);
        if (other.transform.root.gameObject.tag.Equals("Player"))
        {
            PlayerScript player = other.gameObject.GetComponentInParent<PlayerScript>();
            player.ChangeDiamondAmount(amount);
            Destroy(gameObject);
        } else if (other.transform.root.gameObject.tag.Equals("Enemy"))
        {
            try
            {
                BasicResourceCollector resourceCollector = other.gameObject.GetComponentInParent<BasicResourceCollector>();

                resourceCollector.collectResource(type, amount);

                Destroy(gameObject);
            }
            catch { }
        }
    }

    public static void PlayerUpgrade()
    {
        upgraded = true;   
    }
}

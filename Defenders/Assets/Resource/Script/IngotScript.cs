using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngotScript : MonoBehaviour
{
    public ResourceType type;
    public int amount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.transform.root.gameObject.tag);
        if (other.gameObject.transform.root.gameObject.tag.Equals("Player"))
        {
            PlayerScript player = other.gameObject.transform.root.gameObject.GetComponent<PlayerScript>();
            player.SetResourceAmount(type, amount);
            Destroy(gameObject);
        } else if (other.gameObject.transform.root.gameObject.tag.Equals("Enemy"))
        {
            try
            {
                BasicResourceCollector resourceCollector = other.gameObject.transform.root.gameObject.GetComponentInChildren<BasicResourceCollector>();
                resourceCollector.collectResource(type, amount);
                Destroy(gameObject);
            }
            catch { }
        }
    }
}

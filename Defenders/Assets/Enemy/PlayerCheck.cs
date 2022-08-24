using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    public PlayerBasedAIParent aiScript;

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
        if (other.transform.root.tag.Equals("Player"))
        {
            aiScript.PlayerFound(other.GetComponentInParent<PlayerScript>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            print("lost him damn");
            aiScript.PlayerLost(other.GetComponentInParent<PlayerScript>());
        }
    }
}

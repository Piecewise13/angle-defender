using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace_PlayerTrigger : MonoBehaviour
{

    private FurnaceTower script;

    // Start is called before the first frame update
    void Start()
    {
        script = GetComponentInParent<FurnaceTower>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            script.SetPlayer(other.gameObject.GetComponentInParent<PlayerScript>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            script.PlayerExit();
        }
    }

}

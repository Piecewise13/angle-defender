using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
    PlayerScript player;

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
        player = other.GetComponentInParent<PlayerScript>();
    }

    private void OnTriggerStay(Collider other)
    {

        player.SetIsOnLadder(true);

    }

    private void OnTriggerExit(Collider other)
    {
        if (player != null)
        {
            player.SetIsOnLadder(false);
            player = null;
        }
    }

}

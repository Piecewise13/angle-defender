using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFireBall : MonoBehaviour
{

    public int soulFireAmount;
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
            PlayerScript player = other.GetComponentInParent<PlayerScript>();
            player.SetSoulFire(soulFireAmount);
            Destroy(gameObject);
        }
    }

}

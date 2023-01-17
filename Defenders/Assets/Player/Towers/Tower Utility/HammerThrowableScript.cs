using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerThrowableScript : MonoBehaviour
{
    private PlayerScript player;

    private TowerParentScript towerScript;

    private Rigidbody rb;

    public float force;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.gameObject.)
        if (collision.gameObject.CompareTag("Tower"))
        {
            print("hit tower");
            towerScript = collision.gameObject.GetComponentInChildren<TowerParentScript>();
            towerScript.SwitchToTowerCamera(player);
        }
        Destroy(gameObject);
    }

    public void SetPlayer(PlayerScript player)
    {
        this.player = player;
    }


}

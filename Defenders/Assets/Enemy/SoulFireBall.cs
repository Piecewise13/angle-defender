using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFireBall : MonoBehaviour
{

    [SerializeField] private int soulFireAmount;
    public Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    public void SetSoulFire(int value)
    {
        soulFireAmount = value;
    }

    public void LaunchBall(Vector3 direction)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }


}

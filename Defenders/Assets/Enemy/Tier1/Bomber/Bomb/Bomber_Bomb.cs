using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber_Bomb : MonoBehaviour
{

    //private Vector3 direction;
    [SerializeField] private float fuseDuration;
    private float startTime;

    [SerializeField] private float range;
    [SerializeField] private float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fuseDuration + startTime < Time.time)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] walls = Physics.OverlapSphere(transform.position, range, LayerMask.NameToLayer("Defense"));
        foreach (var item in walls)
        {
            WallDefenceScript wallScript = item.GetComponent<WallDefenceScript>();
            wallScript.GiveDamage(damage);
        }


    }

    public void Throw()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunger_PlayerAttack : MonoBehaviour
{

    LungerScript lunger;

    // Start is called before the first frame update
    void Start()
    {
        lunger = GetComponentInParent<LungerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        lunger.StartAttacking(other.GetComponentInParent<Damageable>());

    }

    private void OnTriggerExit(Collider other)
    {
        lunger.StopAttacking();
    }
}

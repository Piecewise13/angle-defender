using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackScript : MonoBehaviour
{

    public BasicEnemyScript main;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.tag.Equals("Enemy"))
        {
            //main.StartAttack(other.GetComponentInParent<Damageable>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.root.tag.Equals("Enemy"))
        {
            main.EndAttack();
        }
    }

}

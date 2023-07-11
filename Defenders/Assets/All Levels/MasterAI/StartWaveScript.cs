using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaveScript : MonoBehaviour, Damageable
{
    public float health { get; set; }
    public bool isDead { get; set; }

    public Animator startAnim;

    Parent_MasterAI master;

    public void Death()
    {
        throw new System.NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        master = GetComponentInParent<Parent_MasterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveDamage(float damage, Collider hitCollider, out float damageGiven, out bool crit)
    {
        damageGiven = 0;
        crit = false;
        GiveDamage(damage);
    }

    public void GiveDamage(float damage)
    {
        //master.Start_Wave();
        if (startAnim)
        {
            startAnim.SetTrigger("HammerFall");
        } else
        {
            master.Start_Wave();
        }

        gameObject.SetActive(false);
    }
}

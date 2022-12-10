using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaveScript : MonoBehaviour, Damageable
{
    public float health { get; set; }
    public bool isDead { get; set; }

    public Animator hammerAnim;

    MasterAI master;

    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage, Collider hitCollider)
    {
        //master.Start_Wave();
        hammerAnim.SetTrigger("HammerFall");
        gameObject.SetActive(false);
    }



    // Start is called before the first frame update
    void Start()
    {
        master = GetComponentInParent<MasterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

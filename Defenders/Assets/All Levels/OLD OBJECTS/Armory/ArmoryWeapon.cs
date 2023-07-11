using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoryWeapon : MonoBehaviour, Damageable
{
    public float health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isDead { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    ModeManager weaponManager;
    
    [SerializeField] private GameObject gun;
    public int wallTier;

    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage, Collider hitCollider)
    {
        //print("hit " + hitCollider + ", gun: " + this.gun);
        //print("Weapon Rack: " + gun);
        //weaponManager.GiveNewGun(this.gun, wallTier);
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ModeManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //print("gun: " + gun);
    }

    public void GiveDamage(float damage, Collider hitCollider, out float damageGiven, out bool crit)
    {
        throw new System.NotImplementedException();
    }

    public void GiveDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}

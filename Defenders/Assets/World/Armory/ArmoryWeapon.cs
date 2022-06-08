using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoryWeapon : MonoBehaviour, Damageable
{
    public float health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isDead { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    WeaponInventoryManager weaponManager;
    
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
        weaponManager.GiveNewGun(this.gun, wallTier);
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<WeaponInventoryManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //print("gun: " + gun);
    }
}

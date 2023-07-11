using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondNodeScript : MonoBehaviour, Damageable
{
    public ResourceType resource;
    public GameObject spawnLocation;
    public ResourceSpawner spawner;

    public float resourceHealth;

    public float health { get; set; }
    public bool isDead { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public GameObject objectSpawn;

    [SerializeField] private float critDamageMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        health = resourceHealth;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal);
        }

    }

    public void dropResource()
    {
        Instantiate(objectSpawn, spawnLocation.transform.position, spawnLocation.transform.rotation);
    }



    public void Death()
    {
        //TODO MAKE SHATTER PARTICLE EFFECT
        spawner.RemoveResource(this);
        dropResource();
        Destroy(gameObject);
    }

    public void RemoveResource()
    {
        //PLAY SINKING BACK INTO GROUND ANIM
        Destroy(gameObject);
    }

    public void GiveDamage(float damage)
    {
        health -= damage;
        //model.material.SetFloat("_CrackValue", Mathf.Lerp(1.5f, 0, health / resourceHealth));
        if (health <= 0)
        {
            Death();

        }
    }

    public void GiveDamage(float damage, Collider hitCollider, out float damageGiven, out bool crit)
    {
        crit = hitCollider.tag == "crit";
        damageGiven = damage * critDamageMultiplier;
        GiveDamage(damage);
    }
}

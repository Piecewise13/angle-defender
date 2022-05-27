using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public abstract class ParentAIScript : MonoBehaviour, Damageable
{
    protected NavMeshAgent agent;
    protected static EggScript eggScript;
    protected static GameObject egg;
    protected static ResourceSpawner resourceSpawner;
    [SerializeField] protected float maxHealth;
    public float health { get; set; }
    public bool isDead { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }



    // Start is called before the first frame update
    protected void initValues()
    {
        health = maxHealth;
        resourceSpawner = FindObjectOfType<ResourceSpawner>();
        eggScript = FindObjectOfType<EggScript>();
        egg = eggScript.transform.root.gameObject;

        agent = GetComponent<NavMeshAgent>();
        
    }

    

    public abstract void takeDamage(float damage, Collider hitCollider);   

    public abstract void death();

    public abstract void reachedEgg();

}

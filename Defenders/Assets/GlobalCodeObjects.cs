using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCodeObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

public interface Damageable
{
    void takeDamage(float damage, Collider hitCollider);
    void death();

    float health { get; set;}
    bool isDead { get; set; }
    
}

public enum upgradeType
{
    Defence,
    Player,
    Resource,
    Weapon
};
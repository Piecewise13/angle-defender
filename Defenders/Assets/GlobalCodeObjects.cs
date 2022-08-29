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
    void TakeDamage(float damage, Collider hitCollider);
    void Death();

    float health { get; set;}
    bool isDead { get; set; }
    
}

public enum UpgradeType
{
    Defence,
    Player,
    Resource,
    Weapon
};

public static class Extns
{
    public static Vector2 xz(this Vector3 vv)
    {
        return new Vector2(vv.x, vv.z);
    }

    public static float FlatDistanceTo(this Vector3 from, Vector3 unto)
    {
        Vector2 a = from.xz();
        Vector2 b = unto.xz();
        return Vector2.Distance(a, b);
    }

    public static Vector3 xz3(this Vector3 vv)
    {
        return new Vector3(vv.x, 0f, vv.z);
    }

}
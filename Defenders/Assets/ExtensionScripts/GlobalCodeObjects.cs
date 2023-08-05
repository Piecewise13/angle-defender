using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalCodeObjects : MonoBehaviour
{


}

public interface Damageable
{
    //returns the damage given 
    void GiveDamage(float damage,
                     Collider hitCollider,
                     out float damageGiven, out bool crit);
    void GiveDamage(float damage);
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

    public static Vector3 LocationWithOthersHeight(Vector3 vec, Vector3 other)
    {
        return vec.xz3() + other.y * Vector3.up;
    }

    public static IEnumerator TextFlash(TMP_Text text)
    {
        text.color = Color.red;
        yield return new WaitForSeconds(.5f);
        text.color = Color.white;
        yield return new WaitForSeconds(.5f);
        text.color = Color.red;
        yield return new WaitForSeconds(.5f);
        text.color = Color.white;
        yield return new WaitForSeconds(.5f);
    }

}

[System.Serializable]
public struct PurchaseWeaponInformation
{
    public Sprite icon;
    public string name;
    public int cost;
    public GameObject weapon;
    public int tier;
}
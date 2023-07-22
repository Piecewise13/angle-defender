using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapon Information")]
public class WeaponInformation : ScriptableObject
{

    public Sprite icon;
    public new string name;
    public int cost;
    public GameObject weapon;
    public int tier;

    public bool Equals(ParentWeaponScript other)
    {
        return this.name.Equals(other.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}

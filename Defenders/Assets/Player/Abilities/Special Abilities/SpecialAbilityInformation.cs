using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Special Ability", menuName = "Scriptable Objects/Abilities/Special", order = 2)]
public class SpecialAbilityInformation : ScriptableObject
{
    public Sprite icon;

    public string abilityName;

    [TextArea]
    public string abilityDescription;

    //add a cool video to decribe it


    [Space(30)]

    public float cooldown;

    //[System.Serializable]
    public MonoScript functionalityScript;

    

}

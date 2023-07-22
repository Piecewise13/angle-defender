using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Ultimate Ability", menuName = "Scriptable Objects/Abilities/Ultimate", order = 2)]
public class UltimateAbilityInformation : ScriptableObject
{
    public Sprite icon;

    public string abilityName;

    [TextArea]
    public string abilityDescription;

    //add a cool video to decribe it


    [Space(30)]

    public float chargeAmount;

    //[System.Serializable]
    public MonoScript functionalityScript;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Range_Fire_MiddleMa : MonoBehaviour
{

    Basic_Ranged_Script script;

    private void Start()
    {
        script = GetComponentInParent<Basic_Ranged_Script>();
    }

    public void Fire()
    {
        script.Shoot();
    }
}

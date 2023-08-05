using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDefenseScript : MonoBehaviour
{

    public static bool showSnapIndicator;

    public GameObject indicator;

    public void Update()
    {
        indicator.SetActive(showSnapIndicator);
    }


}

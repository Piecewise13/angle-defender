using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDemonAnimScript : MonoBehaviour
{


    public FlyingDemonScript main;


    public void StartBomingRun()
    {
        main.Bombing();
    }

    public void SonicAttack()
    {
        main.SonicAttack();
    }

    public void EndSonicAttack()
    {
        main.EndSonicAttack();
    }

    public void StartWallAttack()
    {
        main.WallAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            other.GetComponentInParent<PlayerScript>().Death();
        }
    }

}

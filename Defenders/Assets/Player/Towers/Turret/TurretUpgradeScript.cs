using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretUpgradeScript : MonoBehaviour
{

    public GameObject turret;

    public GameObject spawnPoint;

    public Animator animator;

    protected TurretScript mainScript;




    public void Start()
    {
        mainScript = GetComponentInParent<TurretScript>();
        //mainScript.bulletSpawnPoint = spawnPoint;
        //mainScript.turret = turret;
        mainScript.SetAnimator(animator);
        Upgrade();
    }

    public abstract void Upgrade();
}

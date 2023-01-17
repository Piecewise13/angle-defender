using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_ShootingMiddleMan : MonoBehaviour
{

    private TurretScript script;
    private Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        script = GetComponentInParent<TurretScript>();
        anim = GetComponent<Animator>();
        script.SetAnimator(anim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootMiddleMan()
    {
        script.Shoot();
    }
}

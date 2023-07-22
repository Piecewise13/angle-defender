using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class ParentSpecialAbility : MonoBehaviour {

    protected PlayerScript player;

    public abstract void ActivateAbility();

    public abstract void EndAbility();


    public void Start()
    {
        player = GetComponent<PlayerScript>();
    }

    public virtual void DestroyAbility()
    {
        Destroy(this);
    }

}

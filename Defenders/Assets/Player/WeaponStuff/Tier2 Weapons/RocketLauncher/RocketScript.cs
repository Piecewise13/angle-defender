using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RocketScript : MonoBehaviour
{

    [SerializeField]protected float speed;
    protected Vector3 destination;



    public void SetDestination(Vector3 pos)
    {
        destination = pos;
    }
}

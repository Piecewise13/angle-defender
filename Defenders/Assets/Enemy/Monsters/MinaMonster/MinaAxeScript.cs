using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinaAxeScript : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    private float startTime;

    private Vector3 throwTarget;
    private Vector3 originPos;


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        transform.LookAt(throwTarget);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, throwTarget, speed * Time.deltaTime);
        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, throwTarget) < 0.001f)
        {

            Destroy(gameObject);
            //add fancy destroy
        }
        if (startTime + lifeTime < Time.time)
        {
            Destroy(gameObject);
            //add fancy destroy
        }
    }

    public void SetThrowVars(Vector3 targetPos)
    {
        throwTarget = targetPos;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.tag.Equals("Player"))
        {
            PlayerScript player = other.gameObject.GetComponentInParent<PlayerScript>();
            player.Death();
        }
    }


}

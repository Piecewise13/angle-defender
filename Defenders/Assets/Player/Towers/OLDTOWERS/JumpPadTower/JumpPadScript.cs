using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadScript : MonoBehaviour
{
    private bool isReady = false;
    public float height;

    public float lifeTime;
    private float startLifeTime;


    // Start is called before the first frame update
    void Start()
    {
        startLifeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime + startLifeTime < Time.time)
        {
            print("destorying jump pad");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isReady)
        {
            if (other.transform.root.tag.Equals("Player"))
            {
                PlayerScript script = other.GetComponentInParent<PlayerScript>();
            }
        }

    }

    public void Setup()
    {
        isReady = true;

    }
}

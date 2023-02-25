using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureTowerScript : MonoBehaviour
{

    [SerializeField] private float searchRadius;

    [SerializeField] private float searchTime;
    private float lastSearchTime;

    [SerializeField]private float lifeTime;
    private float startLifeTime;

    private bool isReady;
    public LayerMask layer;

    private void Start()
    {
        startLifeTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (lifeTime + startLifeTime > Time.time)
            {


                if (lastSearchTime + searchTime < Time.time)
                {
                    Collider[] enemies = Physics.OverlapSphere(transform.position, searchRadius, layer);
                    for (int i = 0; i < enemies.Length; i++)
                    {

                        try
                        {
                            ParentAIScript script = enemies[i].GetComponentInParent<ParentAIScript>();
                            script.Lure(transform.position);

                        }
                        catch { }
                    }
                    lastSearchTime = Time.time;
                }
            } else
            {
                Collider[] enemies = Physics.OverlapSphere(transform.position, searchRadius, layer);
                for (int i = 0; i < enemies.Length; i++)
                {
                    try
                    {
                        ParentAIScript script = enemies[i].GetComponentInParent<ParentAIScript>();
                        script.EndLure();
                    }
                    catch { }
                }

                Destroy(gameObject);
            }
        }
    }

    public void Setup()
    {
        isReady = true;
    }
}

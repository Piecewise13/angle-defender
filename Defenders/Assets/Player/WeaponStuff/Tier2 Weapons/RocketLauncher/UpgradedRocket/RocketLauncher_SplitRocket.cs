using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher_SplitRocket : RocketScript
{

    public GameObject splitBombPrefab;
    [SerializeField] private int numberOfSplits;


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        //transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        float degreeDelta = 360f / numberOfSplits;
        for (int i = 0; i < numberOfSplits; i++)
        {
            Instantiate(splitBombPrefab, transform.position + Vector3.up, Quaternion.Euler(0f, i * degreeDelta, 0f));
        }

        Destroy(gameObject);
    }

}

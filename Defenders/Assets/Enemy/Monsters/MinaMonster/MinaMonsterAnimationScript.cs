using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinaMonsterAnimationScript : MonoBehaviour
{

    private MinaMonsterScript mainScript;

    // Start is called before the first frame update
    void Start()
    {
        mainScript = GetComponentInParent<MinaMonsterScript>();
    }



    public void AxeThrow()
    {

        mainScript.AxeThrow();
    }

    public void GrabNewAxe()
    {
        mainScript.GrabNewAxe();
    }

    public void RocksStartMove()
    {
        mainScript.RocksStartMove();
    }

    public void SlamRocks()
    {
        mainScript.SlamRocks();
    }

    public void StartRush()
    {
        mainScript.StartRush();
    }

    public void Rush()
    {
        mainScript.Rush();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

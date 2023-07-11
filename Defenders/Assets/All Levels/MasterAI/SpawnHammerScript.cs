using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHammerScript : MonoBehaviour
{

    Parent_MasterAI masterAI;
    public Animator masterAnim;

    // Start is called before the first frame update
    void Start()
    {
        masterAI = GetComponentInParent<Parent_MasterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartChalice()
    {
        masterAI.Start_Wave();
        masterAnim.SetBool("isSwinging", true);
    }
}

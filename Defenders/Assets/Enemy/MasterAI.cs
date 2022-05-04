using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour
{

    private float ironAmount;
    private float woodAmount;
    private float diamondAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateResourceAmount(ResourceType type, float amount)
    {
        switch (type)
        {
            case (ResourceType.Wood):
                woodAmount += amount;
                break;
            case (ResourceType.Iron):
                ironAmount += amount;
                break;
            case (ResourceType.Diamond):
                diamondAmount += amount;
                break;
        }
    }
}

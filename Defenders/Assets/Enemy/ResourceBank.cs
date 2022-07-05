using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBank : MonoBehaviour
{
    private static MasterAI master;

    // Start is called before the first frame update
    void Start()
    {
        master = FindObjectOfType<MasterAI>();
    }

    public void OnTriggerEnter(Collider other)
    {
        //print("enter");
        if (other.gameObject.transform.root.gameObject.tag.Equals("Enemy"))
        {
            //print("checked");
            try
            {
                //print("trying");
                BasicResourceCollector enemyScript = other.gameObject.GetComponentInParent<BasicResourceCollector>();


                if (enemyScript.collectedAmount > 0)
                {
                    
                    enemyScript.collectedAmount = 0;
                    //master.updateResourceAmount(enemyScript.collectedResource, enemyScript.collectedAmount);
                }

                enemyScript.AssignTarget(ResourceType.Wood);

            }
            catch {
                print("failed");
                return;
            }
        }
    }
}

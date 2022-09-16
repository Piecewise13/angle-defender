using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRepairTowerScript : MonoBehaviour
{
    public int numOfRepairs;
    public float radius;
    public float repairTime;
    private float lastRepairTime;
    public float blinkTime;
    
    private List<WallDefenceScript> wallsInRange = new List<WallDefenceScript>();
    private bool isReady;

    public GameObject beam;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (numOfRepairs > 0)
            {
                if (repairTime + lastRepairTime < Time.time)
                {
                    bool bBlink = false;
                    foreach (var wall in wallsInRange)
                    {
                        if (wall.Repair())
                        {
                            bBlink = true;
                        }


                    }
                    if (bBlink)
                    {
                        StartCoroutine(blink());
                    }
                    numOfRepairs--;
                    lastRepairTime = Time.time;
                }
            } else
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, LayerMask.NameToLayer("Defenses"));
        for (int i = 0; i < hitColliders.Length; i++)
        {
            WallDefenceScript script;

            try
            {
                script = hitColliders[i].GetComponentInParent<WallDefenceScript>();
                if (script != null)
                {
                    wallsInRange.Add(script);
                    print(script);
                }
            }
            catch
            {

                continue;
            }
           // 
            //print(hitColliders[i].gameObject);
        }
        print("Walls " + wallsInRange.Count);
        isReady = true;
    }

    IEnumerator blink()
    {
        beam.SetActive(true);

        yield return new WaitForSeconds(blinkTime);

        beam.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZepplinScript : ParentAIScript
{
    //DroppingVars
    public GameObject hangerPrefab;
    public HangerClass[] hangers;
    private int numOfHangers;
    int numDropped;




    public Collider zepplinCollider;
    public ParticleSystem explosion;
    [SerializeField] private float explosionRadius;


    //Zepplin Movement
    float initDist;
    public float distThreshold;
    public float checkTime;
    float lastCheckTime;
    private bool isDescending;
    [SerializeField] private float speed;
    private float initY;
    private float descentTime;



    








    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        var lookPos = egg.transform.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos, Vector3.up);

        initDist = Extns.FlatDistanceTo(egg.transform.position, transform.position);
        numOfHangers = hangers.Length;
        initY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDescending)
        {
            //make it smoothly go down the more it's shot

            transform.position += (transform.forward * speed * Time.deltaTime);
            transform.position = xz(transform.position) + (Vector3.up * Mathf.Lerp(initY, 0, 1f - (health / maxHealth)));
            //print(Vector3.MoveTowards(transform.position, xz(egg.transform.position), speed * Time.deltaTime) + (Vector3.up * Mathf.Lerp(initY, 0, 1f - (health / maxHealth))));
            // + (transform.up * Mathf.Lerp(, 0f, ));
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, egg.transform.position, (speed/2) * Time.deltaTime);
        }
        

        if (checkTime + lastCheckTime < Time.time)
        {
            float currentDist = Extns.FlatDistanceTo(egg.transform.position, transform.position);
            if (currentDist < distThreshold)
            {

                isDescending = true;
            }
            print(currentDist < (initDist / numOfHangers) * (numOfHangers - numDropped));
            if (currentDist < (initDist / numOfHangers) * (numOfHangers - numDropped))
            {
                foreach (HangerClass item in hangers)
                {
                    if (item.getHealth() > 0)
                    {

                        ParentAIScript aiScript = Instantiate(hangerPrefab, item.getHanger().transform.position, item.getHanger().transform.rotation).GetComponentInChildren<ParentAIScript>();
                        aiScript.health = item.getHealth();

                        item.setHealth(-100f);
                        Destroy(item.getHanger());
                        numDropped++;
                        print("dropping hanger");
                        lastCheckTime = Time.time;
                        break;
                        
                    }
                }
            }

            lastCheckTime = Time.time;
        }
    }



    public override void TakeDamage(float damage, Collider hitCollider)
    {
        if (hitCollider.Equals(zepplinCollider))
        {
            health -= damage;
            if (health <= 0)
            {
                Death();
            }
            return;
        }


        HangerClass hitHanger = FindHanger(hitCollider.gameObject);
        hitHanger.setHealth(-damage);

        if (hitHanger.getHealth() <= 0f)
        {
            speed += 1;
            Destroy(hitCollider.gameObject);
        }

    }

    public override void Death()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.NameToLayer("EntityTrigger"));
        foreach (var item in hitCollider)
        {
            item.gameObject.GetComponentInParent<Damageable>().Death();
        }
        Destroy(gameObject);

    }

    private HangerClass FindHanger(GameObject other)
    {
        foreach (var item in hangers)
        {
            if (item.Equals(other))
            {
                return item;
            }
        }
        return null;
    }

    public Vector3 xz(Vector3 vv)
    {
        return new Vector3(vv.x, 0f, vv.z);
    }


}


[System.Serializable]
public class HangerClass {

    [SerializeField] GameObject hanger;
    [SerializeField]private float health = 50f;

    public  bool Equals(GameObject obj)
    {

        return obj.Equals(hanger);
    }

    public float getHealth()
    {
        return health;

    }

    public void setHealth(float delta)
    {
        health += delta;
    }

    public GameObject getHanger()
    {
        return hanger;
    } 

}

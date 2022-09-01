using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicAttackScript : MonoBehaviour
{


    [HideInInspector]public Vector3 target;
    [SerializeField] private float speed;
    public float endScale;
    private float initDistance;
    private float currentDist;


    // Start is called before the first frame update
    void Start()
    {

        
        transform.LookAt(target);
        transform.localScale = Vector3.one / 10f;
        initDistance = Vector3.Distance(transform.position, target);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
        currentDist = Vector3.Distance(transform.position, target);
        transform.localScale = Vector3.Lerp(Vector3.one* endScale, Vector3.one, currentDist / initDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag.Equals("Player"))
        {
            PlayerScript script = other.GetComponentInParent<PlayerScript>();
            script.SonicAttackEffect();

        }
    }
}

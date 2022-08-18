using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamRockScript : MonoBehaviour
{

    private Vector3 minaPos;
    private Vector3 spawnPos;

    private Vector3 movementDirection;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private static float speed;
    [SerializeField] private float slamSpeed;
    public LayerMask slamLayer;

    private SphereCollider collider;

    public static bool bShouldMove = false;
    public static bool shouldSlam = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;

        movementDirection = (spawnPos - minaPos).normalized;

        speed = Random.Range(minSpeed, maxSpeed);
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldSlam) {
            collider.enabled = true;
            transform.Translate(Vector3.down * slamSpeed * Time.deltaTime);
            return;
        }

        if (bShouldMove) {
            transform.Translate(movementDirection * speed * Time.deltaTime);
        } else
        {
            transform.Translate(Vector3.up * 15f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Collider[] players = Physics.OverlapSphere(transform.position, 10f, slamLayer);
        print("length: " + players.Length);
        if (players.Length > 0)
        {
            foreach (var item in players)
            {
                PlayerScript player = item.GetComponentInParent<PlayerScript>();
                player.Death();
            }
        }
        Destroy(gameObject);
    }


    public void SetMinaPos(Vector3 pos)
    {
        minaPos = pos;
    }
}

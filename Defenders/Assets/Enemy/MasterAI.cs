using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour
{

    public EnemySpawnData[] enemys;

    public Transform[] spawnPoints;


    [SerializeField] private float waveTime;
    private float lastWaveTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].spawnTime = Random.Range(enemys[i].spawnTimeMin, enemys[i].spawnTimeMax);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastWaveTime + waveTime < Time.time)
        {
            foreach (EnemySpawnData enemy in enemys)
            {
                if (enemy.lastSpawnTime + enemy.spawnTime < Time.time)
                {
                    print("Spawning the boyz");
                    Instantiate(enemy.prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
                    enemy.lastSpawnTime = Time.time;
                }
            }
        }
    }


}
[System.Serializable]
public class EnemySpawnData
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public float spawnTimeMin;
    [SerializeField] public float spawnTimeMax;
    [HideInInspector] public float spawnTime;
    [HideInInspector] public float lastSpawnTime;

}

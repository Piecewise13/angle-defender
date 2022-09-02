using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour
{

    public EnemySpawnData[] enemys;
    [SerializeField] private float difficulty;
    float initSpawnRate;
    float firstTierTime;

    int waveNum = 5;
    

    

    public Transform[] spawnPoints;


    [SerializeField] private float waveTime;
    private float lastWaveTime;

    // Start is called before the first frame update
    void Start()
    {
        initSpawnRate = 20f * (1f - (.5f * difficulty));
        firstTierTime = 10f - (5f * difficulty);
        waveTime = SpawnRate();
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastWaveTime + waveTime < Time.time)
        {
            for (int i = 0; i < 5; i++)
            {
                int index = Random.Range(0, enemys.Length);
                if (enemys[index].tier == 2)
                {
                    if (Time.timeSinceLevelLoad/60 > firstTierTime)
                    {
                        Instantiate(enemys[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
                    }
                } else
                {

                    Instantiate(enemys[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
                }



            }
            lastWaveTime = Time.time;
            waveTime = SpawnRate();
        }
    }


    private float SpawnRate()
    {


        return Mathf.Pow((1 - difficulty), (Time.timeSinceLevelLoad/60f) / initSpawnRate * 3 * difficulty) * initSpawnRate;
    }





}
[System.Serializable]
public class EnemySpawnData
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public int tier;

}

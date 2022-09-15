using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour
{

    public EnemySpawnData[] enemysT1;
    public EnemySpawnData[] enemysT2;
    public EnemySpawnData[] enemysT3;




    [SerializeField] private float difficulty;
    float initSpawnRateT1;
    float initSpawnRateT2;
    float initSpawnRateT3;


    [SerializeField] float secondTierTime;
    [SerializeField] float thirdTierTime;


    int waveNumT1 = 7;
    int waveNumT2 = 3;
    int waveNumT3 = 1;




    public Transform[] spawnPoints;


    [SerializeField] private float waveTimeT1;
    [SerializeField] private float waveTimeT2;
    [SerializeField] private float waveTimeT3;

    private float lastWaveTime;
    private float lastWaveTimeT2;
    private float lastWaveTimeT3;

    // Start is called before the first frame update
    void Start()
    {
        initSpawnRateT1 = 20f * (1f - (.5f * difficulty));
        initSpawnRateT2 = 30f * (1f - (.7f * difficulty));
        initSpawnRateT3 = 45f * (1f - (.8f * difficulty));


        secondTierTime = 8f - (5f * difficulty);
        thirdTierTime = 15f - (5f * difficulty);
        waveTimeT1 = SpawnRate();
        waveTimeT2 = SpawnRateT2();
        waveTimeT3 = SpawnRateT3();
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastWaveTime + waveTimeT1 < Time.time)
        {
            for (int i = 0; i < waveNumT1; i++)
            {
                int index = Random.Range(0, enemysT1.Length);

                Instantiate(enemysT1[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
               



            }
            lastWaveTime = Time.time;
            waveTimeT1 = SpawnRate();
        }

        if (Time.timeSinceLevelLoad / 60 > secondTierTime)
        {
            print("Tier two starts spawn");
            if (lastWaveTimeT2 + waveTimeT2 < Time.time)
            {
                for (int i = 0; i < waveNumT2; i++)
                {
                    int index = Random.Range(0, enemysT2.Length);

                    Instantiate(enemysT2[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));




                }
                lastWaveTimeT2 = Time.time;
                waveTimeT2 = SpawnRateT2();
            }
        }


        if (Time.timeSinceLevelLoad / 60 > thirdTierTime)
        {
            print("Tier three starts spawn");
            if (lastWaveTimeT3 + waveTimeT3 < Time.time)
            {
                for (int i = 0; i < waveNumT3; i++)
                {
                    int index = Random.Range(0, enemysT3.Length);

                    Instantiate(enemysT3[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));




                }
                lastWaveTimeT3 = Time.time;
                waveTimeT3 = SpawnRateT3();
            }
        }


    }



    private float SpawnRate()
    {
        //(1-d)^(t/3(S1)(d)) * S1
        return Mathf.Pow((1 - difficulty), (Time.timeSinceLevelLoad/60f) / initSpawnRateT1 * 3 * difficulty) * initSpawnRateT1;
    }

    private float SpawnRateT2()
    {
        //(1-d)^(t/6(S2)(d)) * S2
        return Mathf.Pow((1 - difficulty), (Time.timeSinceLevelLoad / 60f) / initSpawnRateT2 * 6 * difficulty) * initSpawnRateT2;
    }

    private float SpawnRateT3()
    {
        //(1-d)^(t/12(S3)(d)) * S3
        return Mathf.Pow((1 - difficulty), (Time.timeSinceLevelLoad / 60f) / initSpawnRateT3 * 12 * difficulty) * initSpawnRateT3;
    }





}
[System.Serializable]
public class EnemySpawnData
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public int tier;

}

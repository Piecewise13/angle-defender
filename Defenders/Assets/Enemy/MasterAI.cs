using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour
{

    public EnemySpawnData[] enemysT1;
    public EnemySpawnData[] enemysT2;
    public EnemySpawnData[] enemysT3;




    [SerializeField] private float difficulty;
    //float initSpawnRateT1;
    //float initSpawnRateT2;
    //float initSpawnRateT3;


    //[SerializeField] float secondTierTime;
    //[SerializeField] float thirdTierTime;


    //int waveNumT1 = 7;
    //int waveNumT2 = 3;
    //int waveNumT3 = 1;




    public Transform[] spawnPoints;


    //[SerializeField] private float waveTimeT1;
    //[SerializeField] private float waveTimeT2;
    //[SerializeField] private float waveTimeT3;

    //private float lastWaveTime;
    //private float lastWaveTimeT2;
    //private float lastWaveTimeT3;

    [SerializeField] private float timeBetweenWaves;
    private float waveEndTime;
    [SerializeField] private float waveDuration;
    private float waveStartTime;
    private bool shouldSpawn;

    [SerializeField] private float spawnTime;
    private float lastSpawnTime;
    [SerializeField] private float spawnNum;
    private int numOfWaves = 0;

    // Start is called before the first frame update
    void Start()
    {
        //old spawn method
        //initSpawnRateT1 = 20f * (1f - (.5f * difficulty));
        //initSpawnRateT2 = 30f * (1f - (.7f * difficulty));
        //initSpawnRateT3 = 45f * (1f - (.8f * difficulty));


        //secondTierTime = 8f - (5f * difficulty);
        //thirdTierTime = 15f - (5f * difficulty);
        //waveTimeT1 = SpawnRate();
        //waveTimeT2 = SpawnRateT2();
        //waveTimeT3 = SpawnRateT3();

        //new spawn method
        timeBetweenWaves = GetTimeBetweenWaves(numOfWaves);
        waveDuration = GetWaveDuration(numOfWaves);
        spawnTime = GetSpawnTime(numOfWaves);

        spawnPoints = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (shouldSpawn)
        {
            if (waveStartTime + waveDuration < Time.time)
            {
                //wave over
                print("wave over grace time starting");
                shouldSpawn = false;
                numOfWaves++;
                spawnTime = GetSpawnTime(numOfWaves);
                timeBetweenWaves = GetTimeBetweenWaves(numOfWaves);
                waveEndTime = Time.time;
            }
            else
            {
                //start wave
                if (spawnTime + lastSpawnTime < Time.time)
                {
                    if (numOfWaves > 5 && numOfWaves < 15)
                    {
                        //spawn tier 2

                        for (int i = 0; i < spawnNum; i++)
                        {
                            int tier = Random.Range(1, 3);
                            if (tier == 1)
                            {
                                SpawnT1();
                            }
                            else
                            {
                                SpawnT2();
                            }

                        }



                    }
                    else if (numOfWaves > 15)
                    {
                        //spawn tier 3

                        for (int i = 0; i < spawnNum; i++)
                        {
                            int tier = Random.Range(1, 4);
                            if (tier == 1)
                            {
                                SpawnT1();
                            }
                            else if (tier == 2)
                            {
                                SpawnT2();
                            }
                            else
                            {
                                SpawnT3();
                            }

                        }
                    }
                    else
                    {
                        //spawn tier 1

                        for (int i = 0; i < spawnNum; i++)
                        {
                            SpawnT1();
                        }


                    }
                    lastSpawnTime = Time.time;
                }





                //spawn enemies n
            }
        } else
        {
            if (waveEndTime + timeBetweenWaves < Time.time)
            {
                //wave wait time over
                print("wait time over");
                shouldSpawn = true;
                waveDuration = GetWaveDuration(numOfWaves);
                waveStartTime = Time.time;
            }
        }






        //if (lastWaveTime + waveTimeT1 < Time.time)
        //{
        //    for (int i = 0; i < waveNumT1; i++)
        //    {
        //        int index = Random.Range(0, enemysT1.Length);

        //        Instantiate(enemysT1[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
               



        //    }
        //    lastWaveTime = Time.time;
        //    waveTimeT1 = SpawnRate();
        //}

        //if (Time.timeSinceLevelLoad / 60 > secondTierTime)
        //{
        //    print("Tier two starts spawn");
        //    if (lastWaveTimeT2 + waveTimeT2 < Time.time)
        //    {
        //        for (int i = 0; i < waveNumT2; i++)
        //        {
        //            int index = Random.Range(0, enemysT2.Length);

        //            Instantiate(enemysT2[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));




        //        }
        //        lastWaveTimeT2 = Time.time;
        //        waveTimeT2 = SpawnRateT2();
        //    }
        //}


        //if (Time.timeSinceLevelLoad / 60 > thirdTierTime)
        //{
        //    print("Tier three starts spawn");
        //    if (lastWaveTimeT3 + waveTimeT3 < Time.time)
        //    {
        //        for (int i = 0; i < waveNumT3; i++)
        //        {
        //            int index = Random.Range(0, enemysT3.Length);

        //            Instantiate(enemysT3[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));




        //        }
        //        lastWaveTimeT3 = Time.time;
        //        waveTimeT3 = SpawnRateT3();
        //    }
        //}


    }



    //private float SpawnRate()
    //{
    //    //(1-d)^(t/3(S1)(d)) * S1
    //    return Mathf.Pow((1 - difficulty), (Time.timeSinceLevelLoad/60f) / initSpawnRateT1 * 3 * difficulty) * initSpawnRateT1;
    //}

    //private float SpawnRateT2()
    //{
    //    //(1-d)^(t/6(S2)(d)) * S2
    //    return Mathf.Pow((1 - difficulty), (Time.timeSinceLevelLoad / 60f) / initSpawnRateT2 * 6 * difficulty) * initSpawnRateT2;
    //}

    //private float SpawnRateT3()
    //{
    //    //(1-d)^(t/12(S3)(d)) * S3
    //    return Mathf.Pow((1 - difficulty), (Time.timeSinceLevelLoad / 60f) / initSpawnRateT3 * 12 * difficulty) * initSpawnRateT3;
    //}

    private float GetTimeBetweenWaves(int waveNum)
    {
        float c = 40f * (1f - (.5f * difficulty));
        return (Mathf.Pow((1f - difficulty), (3 * numOfWaves) / (c * difficulty))  * c) + 5f;
    }

    private float GetWaveDuration(int waveNum)
    {
        return -4 * Mathf.Log(2 * (waveNum + 1), .5f);
    }

    private float GetSpawnTime(int waveNum)
    {
        return -(((1f - difficulty) / 50f) * waveNum) + (5f * (1f - difficulty) + 2f);
    }

    private void SpawnT1()
    {
        print("SpawnT1");
        int index = Random.Range(0, enemysT1.Length);
        Instantiate(enemysT1[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
    }

    private void SpawnT2()
    {
        print("SpawnT2");
        int index = Random.Range(0, enemysT2.Length);
        Instantiate(enemysT2[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
    }

    private void SpawnT3()
    {
        print("SpawnT3");
        int index = Random.Range(0, enemysT3.Length);
        Instantiate(enemysT3[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
    }

}
[System.Serializable]
public class EnemySpawnData
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public int tier;

}

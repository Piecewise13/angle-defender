using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAI : MonoBehaviour
{

    public EnemySpawnData[] enemysT1;
    public EnemySpawnData[] enemysT2;
    public EnemySpawnData[] enemysT3;




    [SerializeField] private float difficulty;

    int waveNum = 0;
    public List<GameObject> ai = new List<GameObject>();

    public int[] spawnNumber = new int[4];
    float[] spawnTime = new float[4];

    public float randTimeMin;
    public float randTimeMax;

    private bool isSpawning;

    float tier1Time, tier2Time, tier3Time;
    int tier1Number = 0, tier2Number = 0, tier3Number = 0;

    public int numberKilled;
    public int totalToSpawn;
    private int killedMOE = 5;


    public GameObject startWaveObjcet;
    public Transform[] spawnPoints;
    public GameObject spawnBall;
    public Transform spawnBallSpawnPoint;

    public ParticleSystem fireParticles;
    public Animator chaliceAnim;

    public ResourceSpawner resourceSpawner;


    
    // Start is called before the first frame update
    void Start()
    {
        resourceSpawner = FindObjectOfType<ResourceSpawner>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isSpawning)
        {
            //Tier 1
            if (spawnNumber[0] > 0)
            {
                if (spawnTime[0] + tier1Time < Time.time)
                {
                    spawnNumber[0]--;
                    tier1Time = Time.time;
                    Update_SpawnTime(0);
                    SpawnT1();
                }
            }

            if (spawnNumber[1] > 0)
            {
                if (spawnTime[1] + tier2Time < Time.time)
                {
                    spawnNumber[1]--;
                    tier2Time = Time.time;
                    Update_SpawnTime(1);
                    SpawnT2();
                }
            }

            if (spawnNumber[2] > 0)
            {
                if (spawnTime[2] + tier3Time < Time.time)
                {
                    spawnNumber[2]--;
                    tier3Time = Time.time;
                    Update_SpawnTime(2);
                    SpawnT3();
                }
            }

        }

    }


    public void Start_Wave()
    {
        ai.Clear();
        startWaveObjcet.SetActive(false);
        fireParticles.Play();
        waveNum++;
        Update_Number();

        totalToSpawn = 0;
        numberKilled = 0;
        for (int i = 0; i < spawnNumber.Length; i++)
        {
            totalToSpawn += spawnNumber[i];
        }
        

        isSpawning = true;

        tier1Number = 0;
        tier2Number = 0;
        tier3Number = 0;

        tier1Time = Time.time;
        tier2Time = Time.time;
        tier3Time = Time.time;

        resourceSpawner.SpawnResources();

    }

    public void End_Wave()
    {
        isSpawning = false;
        startWaveObjcet.SetActive(true);
        fireParticles.Stop();
        chaliceAnim.SetBool("isSwinging", false);
        resourceSpawner.DespawnResources();
    }

    private void Update_Number()
    {
        spawnNumber[0] = (int)(3f * difficulty * waveNum) + (int)(5 * difficulty) + 10;
        if(waveNum >= 5)
        {
            spawnNumber[1] = (int)(2f * difficulty * waveNum) + (int)(5 * difficulty);
        }
        if (waveNum >= 10)
        {
            spawnNumber[2] = (int)(difficulty * waveNum) - (int)(4 * difficulty);
        }
    }

    private void Update_SpawnTime(int tier)
    {
        spawnTime[tier] = Random.Range(randTimeMin, randTimeMax);
    }

    public void Enemy_Killed(GameObject obj)
    {
        if (ai.Contains(obj))
        {
            ai.Remove(obj);
            numberKilled++;
        }

        if (totalToSpawn - numberKilled <= killedMOE)
        {
            End_Wave();
        }

    }

    private void SpawnT1()
    {
        print("SpawnT1");
        int index = Random.Range(0, enemysT1.Length);
        //Instantiate(enemysT1[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
        SpawnBallScript script = Instantiate(spawnBall, spawnBallSpawnPoint.position, spawnBallSpawnPoint.rotation).GetComponent<SpawnBallScript>();
        script.enemy = enemysT1[index].prefab;
        script.masterAI = this;
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

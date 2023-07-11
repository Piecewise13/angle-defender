using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parent_MasterAI : MonoBehaviour
{

    public EnemySpawnData[] enemysT1;
    public EnemySpawnData[] enemysT2;
    public EnemySpawnData[] enemysT3;
    public EnemySpawnData[] enemysT4;


    [SerializeField] private int maxRoundNumber;

    [Space(20)]
    [Header("Spawn Data")]
    [SerializeField] private float difficulty;
    [SerializeField] private AnimationCurve T1Curve;
    [SerializeField] private int maxNumberT1;
    [SerializeField] private AnimationCurve T2Curve;
    [SerializeField] private int maxNumberT2;
    [SerializeField] private AnimationCurve T3Curve;
    [SerializeField] private int maxNumberT3;
    [SerializeField] private AnimationCurve T4Curve;
    [SerializeField] private int maxNumberT4;

    [Space(20)]
    [Header("Round Data")]
    int roundNumber = 0;
    public List<GameObject> ai = new List<GameObject>();

    public int[] spawnNumber = new int[4];
    float[] spawnTime = new float[4];

    public float randTimeMin;
    public float randTimeMax;

    private bool isSpawning;

    float tier1Time, tier2Time, tier3Time;

    public int numberKilled;
    public int totalToSpawn;
    private int killedMOE = 5;
    private int numberLeft;


    public GameObject startWaveObjcet;
    public Transform[] spawnPoints;


    public ResourceSpawner resourceSpawner;
    private PlayerDataMangerScript playerData;


    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        resourceSpawner = FindObjectOfType<ResourceSpawner>();
        playerData = FindObjectOfType<PlayerDataMangerScript>();
        for (int i = 0; i < spawnTime.Length; i++)
        {
            Update_SpawnTime(i);
        }
       
    }

    // Update is called once per frame
    protected void Update()
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


    public virtual void Start_Wave()
    {
        ai.Clear();
        startWaveObjcet.SetActive(false);

        roundNumber++;
        playerData.UpdateRoundNumber();
        Update_Number();

        totalToSpawn = 0;


        for (int i = 0; i < spawnNumber.Length; i++)
        {
            totalToSpawn += spawnNumber[i];
        }
        

        isSpawning = true;

        tier1Time = Time.time;
        tier2Time = Time.time;
        tier3Time = Time.time;

        resourceSpawner.SpawnResources();

    }

    public virtual void End_Wave()
    {
        isSpawning = false;
        startWaveObjcet.SetActive(true);

        resourceSpawner.DespawnResources();
        numberKilled = 0;
    }
    protected void Update_Number()
    {
        spawnNumber[0] = Mathf.CeilToInt((float)maxNumberT1 * T1Curve.Evaluate((float)roundNumber / (float)maxRoundNumber));
        if(roundNumber >= 3)
        {
            spawnNumber[1] = Mathf.CeilToInt((float)maxNumberT2 * T2Curve.Evaluate((float)(roundNumber - 3) / (float)(maxRoundNumber - 3)));
        }
        if (roundNumber >= 7)
        {
            print((float) T3Curve.Evaluate((float)(roundNumber - 7) / (float)(maxRoundNumber - 7)));
            spawnNumber[2] = Mathf.CeilToInt((float)maxNumberT3 * T3Curve.Evaluate((float)(roundNumber - 7) / (float)(maxRoundNumber - 7)));
        }
        if (roundNumber >= 10)
        {
            spawnNumber[3] = Mathf.CeilToInt((float)maxNumberT4 * T4Curve.Evaluate((float)(roundNumber - 10) / (float)(maxRoundNumber - 10)));
        }
    }

    protected void Update_SpawnTime(int tier)
    {
        spawnTime[tier] = Random.Range(randTimeMin, randTimeMax);
    }

    public void Enemy_Killed(GameObject obj)
    {
        if (ai.Contains(obj))
        {
            ai.Remove(obj);
            numberKilled++;
            numberLeft--;
            playerData.UpdateEnemiesLeft(ai.Count);
        }

        if (numberKilled < 7)
        {
            return;
        }
        if (ai.Count < 3)
        {
            End_Wave();
        }

    }

    public abstract void SpawnT1();

    public abstract void SpawnT2();

    public abstract void SpawnT3();
    protected void SpawnedEnemy()
    {
        numberLeft++;
        playerData.UpdateEnemiesLeft(numberLeft);
    }

    protected Transform GetSpawnpoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }


}
[System.Serializable]
public class EnemySpawnData
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public int tier;

}

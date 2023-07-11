using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_MasterAI : Parent_MasterAI
{

    public GameObject spawnBall;

    public ParticleSystem fireParticles;
    public Animator chaliceAnim;


    public new void Start_Wave()
    {
        fireParticles.Play();
        base.Start_Wave();
    }

    public new void End_Wave()
    {
        fireParticles.Stop();
        chaliceAnim.SetBool("isSwinging", false);

        base.End_Wave();
    }

    public override void SpawnT1()
    {
        Transform spawnpoint = GetSpawnpoint();
        int index = Random.Range(0, enemysT1.Length);
        //Instantiate(enemysT1[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
        SpawnBallScript script = Instantiate(spawnBall, spawnpoint.position, spawnpoint.rotation).GetComponent<SpawnBallScript>();
        script.enemy = enemysT1[index].prefab;
        script.masterAI = this;
        base.SpawnedEnemy();
    }

    public override void SpawnT2()
    {
        Transform spawnpoint = GetSpawnpoint();
        int index = Random.Range(0, enemysT2.Length);
        //Instantiate(enemysT1[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
        SpawnBallScript script = Instantiate(spawnBall, spawnpoint.position, spawnpoint.rotation).GetComponent<SpawnBallScript>();
        script.enemy = enemysT2[index].prefab;
        script.masterAI = this;
        base.SpawnedEnemy();
    }

   public override void SpawnT3()
    {
        Transform spawnpoint = GetSpawnpoint();
        int index = Random.Range(0, enemysT3.Length);
        //Instantiate(enemysT1[index].prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.Euler(Vector3.zero));
        SpawnBallScript script = Instantiate(spawnBall, spawnpoint.position, spawnpoint.rotation).GetComponent<SpawnBallScript>();
        script.enemy = enemysT3[index].prefab;
        script.masterAI = this;
        base.SpawnedEnemy();
    }

}

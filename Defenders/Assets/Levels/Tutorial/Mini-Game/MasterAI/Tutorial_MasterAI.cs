using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_MasterAI : Parent_MasterAI
{

    public override void SpawnT1()
    {
        Transform spawnpoint = GetSpawnpoint();
        int index = Random.Range(0, enemysT1.Length);
        ai.Add(Instantiate(enemysT1[index].prefab, spawnpoint.position, spawnpoint.rotation));
        base.SpawnedEnemy();
    }

    public override void SpawnT2()
    {
        return;
    }

    public override void SpawnT3()
    {
        return;
    }
}

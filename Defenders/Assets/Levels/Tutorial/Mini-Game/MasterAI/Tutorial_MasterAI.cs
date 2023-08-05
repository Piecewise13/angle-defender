using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class Tutorial_MasterAI : Parent_MasterAI
    {
        MiniGameTutorialScript miniGameScript;

        Tutorial_PlayerScript player;

        private bool shouldSpawnResources = false;

        new void Start()
        {
            miniGameScript = FindObjectOfType<MiniGameTutorialScript>();
            player = FindObjectOfType<Tutorial_PlayerScript>();
            base.Start();
        }

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

        public override void Start_Wave()
        {
            miniGameScript.HideText();

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

            if (shouldSpawnResources)
            {
                resourceSpawner.SpawnResources();
            } else
            {
                shouldSpawnResources = true;
            }


        }

        public override void End_Wave()
        {
            if (!isSpawning)
            {
                return;
            }
            miniGameScript.ShowText();
            miniGameScript.DisplayText(player.currentStep + 1);
            isSpawning = false;
            resourceSpawner.DespawnResources();
            numberKilled = 0;
        }

        public void ShowStartSign()
        {
            startWaveObjcet.SetActive(true);
        }

    }
}
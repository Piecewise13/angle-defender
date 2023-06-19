using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataMangerScript : MonoBehaviour
{

    public int monstersToBeat;
    private int monsterBeaten;


    PlayerScript[] players;

    HUDScript[] playerHUDs;

    HashSet<GameObject> defenseLocations = new HashSet<GameObject>();

    private int roundNumber;


    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        players = FindObjectsOfType<PlayerScript>();
        playerHUDs = FindObjectsOfType<HUDScript>();
    }

    public void GameWon()
    {
        SceneManager.LoadScene("WinningScreen");
    }

    public void GameLost()
    {
        SceneManager.LoadScene("LosingScreenScene");
    }

    public void MonsterKilled()
    {
        print("monster beatens");

        monsterBeaten++;
        if (monsterBeaten >= monstersToBeat)
        {
            GameWon();
        }
    }

    public void UpdateRoundNumber()
    {
        roundNumber++;
        foreach (var item in playerHUDs)
        {
            item.UpdateRoundsCounter(roundNumber);
        }
    }

    public void UpdateEnemiesLeft(int number)
    {
        foreach (var item in playerHUDs)
        {
            item.UpdateEnemiesCounter(number);
        }
    }

    public void GiveSoulFire(float amount)
    {
        foreach (var item in players)
        {
            item.SetSoulFire((int)(amount / players.Length));
        }
    }

    public void AddTowerPlacedOnWall(GameObject wall)
    {
        defenseLocations.Add(wall);
    }

    public bool TowerAlreadyPlacedOnWall(GameObject wall)
    {
        return defenseLocations.Contains(wall);
    }

    public void WallDestroyed(GameObject wall)
    {
        defenseLocations.Remove(wall);
    }

}

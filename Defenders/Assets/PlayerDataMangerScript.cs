using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataMangerScript : MonoBehaviour
{

    public int monstersToBeat;
    private int monsterBeaten;


    PlayerScript[] players;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        players = FindObjectsOfType<PlayerScript>();
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

    public void GiveSoulFire(float amount)
    {
        foreach (var item in players)
        {
            item.SetSoulFire((int)(amount / players.Length));
        }
    }

}

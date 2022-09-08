using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataMangerScript : MonoBehaviour
{

    public int monstersToBeat;
    private int monsterBeaten;



    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
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

}

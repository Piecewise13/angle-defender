using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggShopScript : MonoBehaviour
{

    private PlayerScript player;

    public EggScript egg;

    public void Start()
    {
        //egg = GetComponentInParent<EggScript>();
    }

    public PlayerScript GetPlayer()
    {
        return player;
    }

    public void SetPlayerInShop(PlayerScript player)
    {
        this.player = player;
    }
}

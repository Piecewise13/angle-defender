using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class Lobby_PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int connectionID;
    [SyncVar] public int playerIDNumber;
    [SyncVar] public ulong playerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;

    private CustomNetworkManager manager;


    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public void PlayerNameUpdate(string oldValue, string newValue)
    {

    }

}

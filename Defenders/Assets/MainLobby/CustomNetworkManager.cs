using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{

    [SerializeField] private Lobby_PlayerObjectController gamePlayerPrefab;

    public List<Lobby_PlayerObjectController> gamePlayers { get; } = new List<Lobby_PlayerObjectController>();


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name.Equals("Lobby"))
        {
            Lobby_PlayerObjectController gamePlayerInstance = Instantiate(gamePlayerPrefab);
            gamePlayerInstance.connectionID = conn.connectionId;
            gamePlayerInstance.playerIDNumber = gamePlayers.Count + 1;
            gamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.CurrentLobbyID, gamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
        }
    }


}

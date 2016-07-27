using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyManager : NetworkLobbyManager {

  public GameConfig gameConfig;
  public GameObject gameConfigPrefab;

  public override void OnLobbyStartServer ()
  {
    gameConfig = Instantiate (gameConfigPrefab).GetComponent<GameConfig> ();
  }
  
  public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
  {
    Debug.Log ("OnLobbyServerCreateLobbyPlayer");
    Debug.Log (lobbyPlayerPrefab);
    LobbyPlayer player = Instantiate (lobbyPlayerPrefab) as LobbyPlayer;

    Debug.Log (player);

    Debug.Log ("Num Players: " + numPlayers);
    player.playerNum = numPlayers;

    int b = 0, r = 0;
    foreach (LobbyPlayer p in lobbySlots) {
      if (p == null)
        continue;
      if (p.team == "blue")
        b++;
      if (p.team == "red")
        r++;
    }

    if (b > r) {
      player.team = "red";
    } else {
      player.team = "blue";
    }

    NetworkServer.Spawn (gameConfig.gameObject);

    return player.gameObject;
  }

  public override void OnLobbyClientEnter ()
  {
    base.OnLobbyClientEnter ();

    SetupManager.instance.OnLobbyClientEnter ();
  }

  public override void OnStopServer () {
    Debug.Log ("OnStopServer");
    foreach (GameObject player in GameObject.FindGameObjectsWithTag ("GamePlayer")) {
      Destroy (player);
    }
  }

  public override void OnLobbyServerPlayersReady()
  {
    Debug.Log ("OnLobbyServerPlayersReady");
    bool allready = true;
    for(int i = 0; i < lobbySlots.Length; ++i)
    {
      if(lobbySlots[i] != null)
        allready &= lobbySlots[i].readyToBegin;
    }

    if (allready) {
      Debug.Log ("All Ready!");
      base.OnLobbyServerPlayersReady ();
    }
  }

  public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
  {
    Debug.Log ("OnLobbyServerSceneLoadedForPlayer");

    NetworkPlayer networkPlayer = gamePlayer.GetComponent<NetworkPlayer> ();
    LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer> ();
    networkPlayer.name = lPlayer.name;
    networkPlayer.playerNum = lPlayer.playerNum;

    return true;
  }

  public override GameObject OnLobbyServerCreateGamePlayer (NetworkConnection conn, short playerControllerId)
  {
    Debug.Log ("OnLobbyServerCreateGamePlayer");
    return base.OnLobbyServerCreateGamePlayer (conn, playerControllerId);
  }

  public void TeamSizeChanged (int newVal) {
    gameConfig.teamSize = newVal;
  }

  public void GameLengthChanged (int newVal) {
    gameConfig.gameLength = newVal;
  }
}

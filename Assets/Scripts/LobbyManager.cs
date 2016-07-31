using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyManager : NetworkLobbyManager {

  public GameConfig gameConfig;
  public GameObject gameConfigPrefab;
  public bool isPublicServer;

  public string apiUrl = "https://space-arena-api.herokuapp.com/game_hosts";
  public int gameHostId;
  public string gameHostToken;

  public override void OnLobbyStartServer ()
  {
    gameConfig = Instantiate (gameConfigPrefab).GetComponent<GameConfig> ();
    StartCoroutine (RegisterServer ());
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

    StartCoroutine (UpdateServer ());

    return player.gameObject;
  }

  public override void OnLobbyClientEnter ()
  {
    base.OnLobbyClientEnter ();

    SetupManager.instance.OnLobbyClientEnter ();
  }

  public override void OnLobbyServerDisconnect (NetworkConnection conn)
  {
    base.OnLobbyServerDisconnect (conn);

    StartCoroutine (UpdateServer ());
  }

  public override void OnStopServer () {
    Debug.Log ("OnStopServer");
    foreach (GameObject player in GameObject.FindGameObjectsWithTag ("GamePlayer")) {
      Destroy (player);
    }

    StartCoroutine (DestroyServer ());
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
      StartCoroutine (DestroyServer ());
    }
  }

  public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
  {
    Debug.Log ("OnLobbyServerSceneLoadedForPlayer");

    NetworkPlayer networkPlayer = gamePlayer.GetComponent<NetworkPlayer> ();
    LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer> ();
    networkPlayer.name = lPlayer.name;
    networkPlayer.playerNum = lPlayer.playerNum;
    networkPlayer.team = lPlayer.team;

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

  IEnumerator RegisterServer () {
    if (!isPublicServer)
      yield break;

    WWWForm form = new WWWForm ();
    form.AddField ("game_host[version]", "0");
    form.AddField ("game_host[name]", "---");
    form.AddField ("game_host[port]", networkPort.ToString ());
    form.AddField ("game_host[cur_players]", "0");
    form.AddField ("game_host[max_players]", "0");

    WWW w = new WWW (apiUrl, form);
    yield return w;
    if (!string.IsNullOrEmpty (w.error)) {
      Debug.LogError (w.error);
    } else {
      Debug.Log (w.text);
      GameHost gameHost = JsonUtility.FromJson<GameHost> (w.text);
      gameHostToken = gameHost.token;
      gameHostId = gameHost.id;
    }
  }

  IEnumerator UpdateServer () {
    yield break;
  }

  IEnumerator DestroyServer () {
    if (gameHostToken == null || gameHostId == null)
      yield break;

    WWWForm form = new WWWForm ();
    form.AddField ("_method", "DELETE");
    form.AddField ("token", gameHostToken);

    WWW w = new WWW (apiUrl + "/" + gameHostId.ToString (), form);
    yield return w;
    if (!string.IsNullOrEmpty (w.error)) {
      Debug.LogError (w.error);
    } else {
      Debug.Log (w.text);
    }
  }
}

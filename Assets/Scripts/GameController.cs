using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameController : NetworkBehaviour {
  public GameObject shipPrefab;
  public Player aiPlayerPrefab;
  public GameObject[] blueSpawns;
  public GameObject[] redSpawns;
  public int startWait = 3;
  public int roundWait = 3;
  public int minPlayers = 2;

  private GameObject[] players;

  public static GameController instance;

  [SyncVar]
  public bool gameStarted, gameStarting;
  [SyncVar]
  public bool roundStarted, roundStarting, roundEnding;
  [SyncVar]
  public int countdown;


  // Use this for initialization
  void Start () {
    Debug.Log ("GameController#Start");
    instance = this;
    if (!isServer) {
      return;
    }

    Debug.Log ("GameController#Start - isServer");

    countdown = 0;
  }

  void createAIPlayer () {
    Player player = Instantiate (aiPlayerPrefab, Vector3.zero, Quaternion.identity) as Player;
    Debug.Log (player);
    Debug.Log (players);
    player.Name = "NPC " + players.Length;
    player.playerNum = players.Length;
    players = GameObject.FindGameObjectsWithTag ("GamePlayer");
    NetworkServer.Spawn (player.gameObject);
  }

  // Update is called once per frame
  void Update () {
    if (!isServer) {
      return;
    }

    if (!gameStarted && !gameStarting) {
      StartCoroutine (StartGame());
    }

    if (gameStarted) {
      if (roundStarted) {
        int playersRemaining = 0;
        foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag ("Player")) {
          PlayerController playerController = (PlayerController)playerObj.GetComponent<PlayerController>();
          if (playerController && !playerController.isDead)
            playersRemaining += 1;
        }
        if (playersRemaining <= 1 && !roundEnding) {
          StartCoroutine (EndRound ());
        }
      } else if (!roundStarting) {
        StartCoroutine (StartRound());
      }
    }
  }

  IEnumerator StartGame () {
    Debug.Log ("Game Starting");
    gameStarting = true;
    countdown = startWait;
    while (countdown > 0) {
      yield return new WaitForSeconds (1);
      countdown--;
    }
    gameStarting = false;
    players = GameObject.FindGameObjectsWithTag ("GamePlayer");
    while (players.Length < minPlayers)
      createAIPlayer ();

    RespawnAll ();
    gameStarted = true;
  }

  IEnumerator StartRound() {
    Debug.Log ("Round Starting");
    roundStarting = true;
    countdown = roundWait;
    while (countdown > 0) {
      yield return new WaitForSeconds (1);
      countdown--;
    }
    roundStarting = false;

    roundStarted = true;
  }

  IEnumerator EndRound() {
    Debug.Log ("Round Ending");
    roundEnding = true;
    countdown = roundWait;
    while (countdown > 0) {
      yield return new WaitForSeconds (1);
      countdown--;
    }
    roundEnding = false;
    foreach (GameObject player in GameObject.FindGameObjectsWithTag ("Player")) {
      ((PlayerController)player.GetComponent<PlayerController>()).isDead = true;
    }
    RespawnAll ();
    roundStarted = false;
  }

  public void RespawnAll () {
    int b = 0, r = 0;
    GameObject spawn;
    for (var i = 0; i < players.Length; i++) {
      Debug.Log (players [i]);
      Player playerComponent = players [i].GetComponent<Player> ();

      if (i < players.Length / 2) {
        playerComponent.team = "blue";
        spawn = blueSpawns [b];
        b++;
      } else {
        playerComponent.team = "red";
        spawn = redSpawns [r];
        r++;
      }

      Respawn (playerComponent, spawn);
    }
  }

  public void Respawn (Player player, GameObject spawnPoint) {
    if (!isServer)
      return;

    Debug.Log ("Spawn At: " + spawnPoint.transform.position);
    var ship = player.Ship;
    if (ship != null) {
      player.Ship.transform.position = spawnPoint.transform.position;
      player.Ship.transform.rotation = spawnPoint.transform.rotation;
    } else {
      ship = player.Ship = Instantiate (shipPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
      ship.GetComponent<PlayerController> ().playerNum = player.playerNum;
      NetworkServer.Spawn (ship);
    }

    PlayerController playerController = ship.GetComponent<PlayerController> ();
    playerController.OnRespawn (spawnPoint);
    playerController.playerName = player.GetComponent<Player> ().Name;

    player.Replace (ship);
  }

  public bool IsGameRunning () {
    return gameStarted && roundStarted;
  }
}

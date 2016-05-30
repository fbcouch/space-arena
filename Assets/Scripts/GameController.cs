﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameController : NetworkBehaviour {
  public GameObject shipPrefab;
  public NetworkLobbyPlayer aiPlayerPrefab;
  public GameObject[] spawnPoints;
  public int startWait;
  public int roundWait;
  public float spawnRadius;

  private GameObject[] players;


  [SyncVar]
  public bool gameStarted, gameStarting;
  [SyncVar]
  public bool roundStarted, roundStarting, roundEnding;
  [SyncVar]
  public int countdown;


  // Use this for initialization
  void Start () {
    Debug.Log ("GameController#Start");
    if (!isServer) {
      return;
    }

    Debug.Log ("GameController#Start - isServer");

    players = GameObject.FindGameObjectsWithTag ("GamePlayer");
    if (players.Length < 2)
      createAIPlayer ();
    countdown = 0;
  }

  void createAIPlayer () {
    NetworkLobbyPlayer lobbyPlayer = Instantiate (aiPlayerPrefab, Vector3.zero, Quaternion.identity) as NetworkLobbyPlayer;
    var player = lobbyPlayer.gameObject.GetComponent<Player> ();
    player.Name = "NPC " + players.Length;
    players = GameObject.FindGameObjectsWithTag ("GamePlayer");
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
    foreach (GameObject player in players) {
      Respawn (player.GetComponent<Player> ());
    }
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
    roundStarted = false;
  }

  public void Respawn (Player player) {
    if (!isServer)
      return;

    Vector2 spawnLocation = Random.insideUnitCircle * spawnRadius;
    Debug.Log ("Spawn At: " + spawnLocation);
    var ship = player.Ship;
    if (ship != null) {
      Debug.Log ("Move...");
      player.Ship.transform.position = new Vector3 (spawnLocation.x, 0, spawnLocation.y);
    } else {
      Debug.Log ("Instantiate...");
      ship = player.Ship = Instantiate (shipPrefab, new Vector3 (spawnLocation.x, 0, spawnLocation.y), Quaternion.identity) as GameObject;
    }

    PlayerController playerController = ship.GetComponent<PlayerController> ();
    playerController.OnRespawn ();
    playerController.playerName = player.GetComponent<Player> ().Name;

    player.transform.LookAt(Vector3.zero);
    Debug.Log ("Spawn Rotation: " + player.transform.rotation.eulerAngles);
    player.Replace (ship);
  }
}

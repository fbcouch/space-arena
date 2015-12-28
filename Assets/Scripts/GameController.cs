using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameController : NetworkBehaviour {
	public GameObject enemyPrefab;
	public int startWait;
	public int roundWait;

	private ArrayList players;

	[SyncVar]
	public bool gameStarted, gameStarting;
	[SyncVar]
	public bool roundStarted, roundStarting, roundEnding;
	[SyncVar]
	public int countdown;


	// Use this for initialization
	void Start () {
		Debug.Log ("GameController#Start");
		DontDestroyOnLoad (this);
		if (!isServer) {
			return;
		}

		Debug.Log ("GameController#Start - isServer");

		players = ((PositionSpawnNetworkManager)GameObject.FindGameObjectWithTag ("NetworkManager").GetComponent<PositionSpawnNetworkManager> ()).players;
		countdown = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}

		if (!gameStarted && !gameStarting && players.Count >= 2) {
			StartCoroutine (StartGame());
		}

		if (gameStarted) {
			if (roundStarted) {
				int playersRemaining = 0;
				foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag ("Player")) {
					if (!((PlayerController)playerObj.GetComponent<PlayerController>()).isDead)
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
		foreach (Player player in players) {
			Respawn (player);
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
		foreach (Player player in players) {
			if (player.Ship)
				((PlayerController)player.Ship.GetComponent<PlayerController>()).isDead = true;
//			NetworkServer.Destroy (player);
		}
		roundStarted = false;
	}

	public void Respawn (Player player) {
		if (!isServer) {
			return;
		}
		Vector2 spawnLocation = Random.insideUnitCircle * 10;
		Debug.Log ("Spawn At: " + spawnLocation);
		var ship = player.Ship;
		if (ship != null) {
			ship.transform.position = new Vector3 (spawnLocation.x, 0, spawnLocation.y);
		} else {
			ship = player.Ship = Instantiate (enemyPrefab, new Vector3 (spawnLocation.x, 0, spawnLocation.y), Quaternion.identity) as GameObject;
		}

		ship.transform.LookAt(Vector3.zero);
		Debug.Log ("Spawn Rotation: " + ship.transform.rotation.eulerAngles);
		player.Replace (ship);
	}

	public void AddPlayer (Player player) {
		Debug.Log ("GameController#AddPlayer");
		if (!isServer) {
			return;
		}
		Debug.Log ("GameController#AddPlayer - isServer");
		if (players == null)
			players = new ArrayList();
		players.Add (player);
	}
}

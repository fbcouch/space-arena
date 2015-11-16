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
		if (!isServer) { 
			return;
		}

		players = new ArrayList ();
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
				GameObject[] playersRemaining = GameObject.FindGameObjectsWithTag ("Player");
				if (playersRemaining.Length <= 1 && !roundEnding) {
					StartCoroutine (EndRound (playersRemaining));
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
		foreach (NetworkConnection conn in players) {
			Respawn (conn);
		}
		roundStarted = true;
	}

	IEnumerator EndRound(GameObject[] players) {
		Debug.Log ("Round Ending");
		roundEnding = true;
		countdown = roundWait;
		while (countdown > 0) {
			yield return new WaitForSeconds (1);
			countdown--;
		}
		roundEnding = false;
		foreach (GameObject player in players) {
			NetworkServer.Destroy (player);
		}
		roundStarted = false;
	}

	public void SpawnEnemy() {
		Vector3 spawnLocation = new Vector3 (0, 0, 10); // Random.insideUnitSphere * 10;
		Debug.Log ("Spawn Enemy At: " + spawnLocation);
		GameObject enemy = Instantiate (enemyPrefab, spawnLocation, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (enemy);
	}

	public void Respawn (NetworkConnection connectionToClient) {
		if (!isServer) {
			return;
		}

		Vector3 spawnLocation = Random.insideUnitSphere * 100;
		Debug.Log ("Spawn At: " + spawnLocation);
		var player = (GameObject)GameObject.Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
		player.transform.LookAt(Vector3.zero);
		Debug.Log ("Spawn Rotation: " + player.transform.rotation.eulerAngles);
		NetworkServer.ReplacePlayerForConnection (connectionToClient, player, 0);
	}

	public void AddPlayer (NetworkConnection connectionToClient) {
		players.Add (connectionToClient);
	}
}

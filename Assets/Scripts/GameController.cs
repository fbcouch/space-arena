using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameController : NetworkBehaviour {
	public GameObject enemyPrefab;
	public int startWait;
	public int roundWait;

	private ArrayList players;
	private bool gameStarted, gameStarting;
	private bool roundStarted, roundStarting;


	// Use this for initialization
	void Start () {
		if (!isServer) { 
			return;
		}

		players = new ArrayList ();
		gameStarted = false;
		roundStarted = false;
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
				if (playersRemaining.Length <= 1) {
					StartCoroutine (EndRound (playersRemaining));
				}
			} else if (!roundStarting) { 
				StartCoroutine (StartRound());
			}
		}
	}

	IEnumerator StartGame () {
		gameStarting = true;
		yield return new WaitForSeconds (startWait);
		gameStarting = false;
		gameStarted = true;
		Debug.Log ("Game Starting");
	}

	IEnumerator StartRound() {
		roundStarting = true;
		yield return new WaitForSeconds (roundWait);
		roundStarting = false;
		foreach (NetworkConnection conn in players) {
			Respawn (conn);
		}
		roundStarted = true;
		Debug.Log ("Round Starting");
	}

	IEnumerator EndRound(GameObject[] players) {
		yield return new WaitForSeconds (roundWait);
		foreach (GameObject player in players) {
			NetworkServer.Destroy (player);
		}
		roundStarted = false;
		Debug.Log ("Round Ending");
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

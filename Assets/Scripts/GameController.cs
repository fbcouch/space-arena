using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameController : NetworkBehaviour {
	public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
		if (!isServer) { 
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame () {
		SpawnEnemy ();
	}

	public void SpawnEnemy() {
		Vector3 spawnLocation = new Vector3 (0, 0, 10); // Random.insideUnitSphere * 10;
		Debug.Log ("Spawn Enemy At: " + spawnLocation);
		GameObject enemy = Instantiate (enemyPrefab, spawnLocation, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (enemy);
	}

	public void Respawn (NetworkConnection connectionToClient) {
		Vector3 spawnLocation = new Vector3 (0, 0, -10); // Random.insideUnitSphere * 10;
		Debug.Log ("Spawn At: " + spawnLocation);
		var player = (GameObject)GameObject.Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
		NetworkServer.ReplacePlayerForConnection (connectionToClient, player, 0);
	}
}

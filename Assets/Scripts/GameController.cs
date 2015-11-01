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

	void SpawnEnemy() {
		Vector3 spawnLocation = new Vector3 (0, 0, 10); // Random.insideUnitSphere * 10;
		Debug.Log ("Spawn Enemy At: " + spawnLocation);
		GameObject.Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
	}
}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PositionSpawnNetworkManager : NetworkManager {

	public override void OnClientConnect (NetworkConnection conn) {
		Debug.Log ("Client connected");
		base.OnClientConnect (conn);
		GameController gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		gameController.StartGame ();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Vector3 spawnLocation = new Vector3 (0, 0, -10); // Random.insideUnitSphere * 10;
		Debug.Log ("Spawn At: " + spawnLocation);
//		var player = (GameObject)GameObject.Instantiate(playerPrefab, spawnLocation, Quaternion.identity);
//		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		GameController gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		gameController.Respawn (conn);
	}
}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PositionSpawnNetworkManager : NetworkManager {

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameController gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		NetworkPlayer player = new NetworkPlayer ();
		player.Connection = conn;
		gameController.AddPlayer (player);

		gameController.AddPlayer (new Player ());
	}
}

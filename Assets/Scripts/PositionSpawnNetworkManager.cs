using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PositionSpawnNetworkManager : NetworkManager {

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameController gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		gameController.AddPlayer (conn);
	}
}

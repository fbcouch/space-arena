using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : Player {
	NetworkConnection connection;

	public override void Replace (GameObject ship) {
		Ship = ship;
		PlayerController playerController = (PlayerController)ship.GetComponent<PlayerController> ();
		playerController.player = this;
		playerController.isDead = false;
		//NetworkServer.ReplacePlayerForConnection (connection, ship, 0);
	}

	public NetworkConnection Connection {
		get {
			return this.connection;
		}
		set {
			connection = value;
		}
	}
}

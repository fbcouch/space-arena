using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : Player {
	NetworkConnection connection;

	public override void Replace (GameObject ship) {
		Ship = ship;
		NetworkServer.ReplacePlayerForConnection (connection, ship, 0);
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

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player {
	GameObject ship;

	public virtual void Replace (GameObject ship) {
		Ship = ship;
		NetworkServer.Spawn (ship);
	}

	public GameObject Ship {
		get {
			return this.ship;
		}
		set {
			ship = value;
		}
	}
}

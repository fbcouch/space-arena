using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player {
	GameObject ship;

	public virtual void Replace (GameObject ship) {
		if (ship != Ship) {
			NetworkServer.Spawn (ship);
		}
		Ship = ship;
		PlayerController playerController = (PlayerController)ship.GetComponent<PlayerController> ();
		playerController.player = this;
		playerController.isDead = false;
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

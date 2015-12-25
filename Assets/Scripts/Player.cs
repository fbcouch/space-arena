using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player {
	GameObject ship;

	public int kills = 0;
	public int deaths = 0;

	public virtual void Replace (GameObject ship) {
		Ship = ship;
		((PlayerController)ship.GetComponent<PlayerController> ()).player = this;
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

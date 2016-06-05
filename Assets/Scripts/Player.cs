using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
  public GameObject ship;
  [SyncVar]
  public string name;
  [SyncVar]
  public int playerNum = -1;

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

  public string Name {
    get {
      return this.name;
    }
    set {
      name = value;
    }
  }

  public virtual void FireWeapons() {
    Debug.Log ("Fire Weapons?");
  }
}

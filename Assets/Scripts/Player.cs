﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
  public GameObject ship;
  [SyncVar]
  public string name;
  [SyncVar]
  public int playerNum = -1;
  [SyncVar]
  public string shipIdentifier;

  public float throttle = 0;
  public float pitch = 0;
  public float roll = 0;
  public float yaw = 0;
  [SyncVar]
  public bool fire1 = false;
  [SyncVar]
  public bool fire2 = false;
  public int averagePing = 0;
  [SyncVar]
  public string team;

  public bool serverControl = false;

  public void RandomizeShip () {
    shipIdentifier = ShipDataHolder.instance.shipData [Random.Range (0, ShipDataHolder.instance.shipData.Length - 1)].identifier;
  }

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

  [ClientRpc]
  public void RpcFireWeapons (float timeRemaining) {
    if (isLocalPlayer)
      return;
    PlayerController playerController = (PlayerController)ship.GetComponent<PlayerController> ();
    if (playerController != null)
      playerController.FireWeapons (timeRemaining - GameController.instance.timeRemaining);
  }

  [Command]
  public void CmdFireWeapons (float timeRemaining) {
    RpcFireWeapons (timeRemaining);
  }
}
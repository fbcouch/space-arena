﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : Player {
  float lastThrottle;
	NetworkConnection connection;

	public override void Replace (GameObject ship) {
		Ship = ship;
		PlayerController playerController = (PlayerController)ship.GetComponent<PlayerController> ();
		playerController.player = this;
		playerController.isDead = false;
		//NetworkServer.ReplacePlayerForConnection (connection, ship, 0);
    ship.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
	}

	public NetworkConnection Connection {
		get {
			return this.connection;
		}
		set {
			connection = value;
		}
	}

  public override void FireWeapons () {
    Debug.Log ("Fire Weapons!");
    if (isLocalPlayer)
      CmdFireWeapons ();
  }

  public void Start () {
    if (!isLocalPlayer)
      return;
    StartCoroutine (SendPing ());
  }

  public void FixedUpdate () {
    if (!isLocalPlayer)
      return;

    if (Input.GetAxis ("Horizontal") != roll) {
      roll = Input.GetAxis ("Horizontal");
    }
    if (Input.GetAxis ("Vertical") != roll) {
      pitch = Input.GetAxis ("Vertical");
    }
    if (Input.GetAxis ("Rudder") != roll) {
      yaw = Input.GetAxis ("Rudder");
    }
    if (Input.GetAxis ("Throttle") != lastThrottle) {
      lastThrottle = throttle = (Input.GetAxis ("Throttle") + 1) / 2;
    }
    if (Input.GetAxis ("ThrottleKey") > 0.1) {
      throttle += 0.01f;
    }
    if (Input.GetAxis ("ThrottleKey") < -0.1) {
      throttle -= 0.01f;
    }
    throttle = Mathf.Clamp (throttle, 0, 1);

    fire1 = Input.GetButton ("Fire1");
    fire2 = Input.GetButton ("Fire2");
  }

  IEnumerator SendPing () {
    while (true) {
      CmdSetPing (Network.GetAveragePing (Network.player));
      yield return new WaitForSeconds (1);
    }
  }

  [Command]
  public void CmdSetPing (int ping) {
    averagePing = ping;
  }

  [Command]
  public void CmdSetInput(string name, float value) {
    switch (name) {
    case "roll":
      roll = value;
      break;
    case "yaw":
      yaw = value;
      break;
    case "pitch":
      pitch = value;
      break;
    case "throttle":
      throttle = value;
      break;
    default:
      Debug.Log ("Unknown Input: " + name);
      break;
    }
  }

  [Command]
  public void CmdSetTeam(string name) {
    Debug.Log ("Set team to " + name);
  }
}

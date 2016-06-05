﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIPlayerSlot : MonoBehaviour {
	public NetworkPlayer player;
	public NetworkLobbyPlayer lobbyPlayer;
	public Text playerName;
	public Toggle playerReady;

  bool wasOn = true;

	// Use this for initialization
	void Start () {
		playerName.text = "Empty";
		playerReady.isOn = false;
	}
	
	// Update is called once per frame
	void Update () {
    if (player == null || lobbyPlayer == null) {
      playerReady.interactable = false;
      return;
    }
		
    if (lobbyPlayer.isLocalPlayer) {
      playerReady.interactable = true;
      lobbyPlayer.readyToBegin = playerReady.isOn;
      if (playerReady.isOn && !wasOn) {
        wasOn = true;
        lobbyPlayer.SendReadyToBeginMessage ();
      }

      if (!playerReady.isOn && wasOn) {
        wasOn = false;
        lobbyPlayer.SendNotReadyToBeginMessage ();
      }
    } else {
      playerReady.interactable = false;
    }

		playerName.text = player.Name;
		playerReady.isOn = lobbyPlayer.readyToBegin;
	}
}

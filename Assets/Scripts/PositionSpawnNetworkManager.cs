using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PositionSpawnNetworkManager : NetworkLobbyManager {
  public UIPlayerSlot[] uiSlots;

  public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
  {
    Debug.Log ("OnLobbyServerCreateLobbyPlayer");
    Debug.Log (lobbyPlayerPrefab);
    NetworkLobbyPlayer player = Instantiate (lobbyPlayerPrefab, Vector3.zero, Quaternion.identity) as NetworkLobbyPlayer;

    Debug.Log (player);
    NetworkPlayer networkPlayer = player.gameObject.GetComponent<NetworkPlayer> ();
    networkPlayer.Connection = conn;

	  if (numPlayers < uiSlots.Length) {
			Debug.Log ("Num Players: " + numPlayers);
			Debug.Log (uiSlots [numPlayers]);
	    uiSlots [numPlayers].player = networkPlayer;
	    uiSlots [numPlayers].lobbyPlayer = player;
      networkPlayer.playerNum = numPlayers;
	  }

    return player.gameObject;
  }

  public override void OnStopServer () {
    Debug.Log ("OnStopServer");
    foreach (GameObject player in GameObject.FindGameObjectsWithTag ("GamePlayer")) {
      Destroy (player);
    }
  }
}

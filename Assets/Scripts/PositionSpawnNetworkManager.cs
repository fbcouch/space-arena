using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PositionSpawnNetworkManager : NetworkLobbyManager {
  public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
  {
    Debug.Log ("OnLobbyServerCreateLobbyPlayer");
    Debug.Log (lobbyPlayerPrefab);
    NetworkLobbyPlayer player = Instantiate (lobbyPlayerPrefab, Vector3.zero, Quaternion.identity) as NetworkLobbyPlayer;

    Debug.Log (player);
    NetworkPlayer networkPlayer = player.gameObject.GetComponent<NetworkPlayer> ();
    networkPlayer.Connection = conn;
    networkPlayer.Name = "Player " + numPlayers;
    return player.gameObject;
  }

  public override void OnStopServer () {
    Debug.Log ("OnStopServer");
    foreach (GameObject player in GameObject.FindGameObjectsWithTag ("GamePlayer")) {
      Destroy (player);
    }
  }
}

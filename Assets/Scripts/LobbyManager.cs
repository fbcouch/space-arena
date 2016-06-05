using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyManager : NetworkLobbyManager {
  public UIPlayerSlot[] uiSlots;

  public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
  {
    Debug.Log ("OnLobbyServerCreateLobbyPlayer");
    Debug.Log (lobbyPlayerPrefab);
    LobbyPlayer player = Instantiate (lobbyPlayerPrefab) as LobbyPlayer;

    Debug.Log (player);

	  if (numPlayers < uiSlots.Length) {
			Debug.Log ("Num Players: " + numPlayers);
			Debug.Log (uiSlots [numPlayers]);
	    uiSlots [numPlayers].lobbyPlayer = player;
      player.playerNum = numPlayers;
	  }

    return player.gameObject;
  }

  public override void OnStopServer () {
    Debug.Log ("OnStopServer");
    foreach (GameObject player in GameObject.FindGameObjectsWithTag ("GamePlayer")) {
      Destroy (player);
    }
  }

  public override void OnLobbyServerPlayersReady()
  {
    Debug.Log ("OnLobbyServerPlayersReady");
    bool allready = true;
    for(int i = 0; i < lobbySlots.Length; ++i)
    {
      if(lobbySlots[i] != null)
        allready &= lobbySlots[i].readyToBegin;
    }

    if (allready) {
      Debug.Log ("All Ready!");
      base.OnLobbyServerPlayersReady ();
    }
  }

  public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
  {
    Debug.Log ("OnLobbyServerSceneLoadedForPlayer");

    NetworkPlayer networkPlayer = gamePlayer.GetComponent<NetworkPlayer> ();
    LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer> ();
    networkPlayer.name = lPlayer.name;

    return true;
  }

  public override GameObject OnLobbyServerCreateGamePlayer (NetworkConnection conn, short playerControllerId)
  {
    Debug.Log ("OnLobbyServerCreateGamePlayer");
    return base.OnLobbyServerCreateGamePlayer (conn, playerControllerId);
  }
}

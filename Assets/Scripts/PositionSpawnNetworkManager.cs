using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PositionSpawnNetworkManager : NetworkManager {
	public ArrayList players = new ArrayList();

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		NetworkPlayer player = new NetworkPlayer ();

//		Player other = new Player ();
//		other.Name = "Player 2";
//		players.Add (other);

		player.Connection = conn;
		player.Name = "Player " + playerControllerId;
		players.Add (player);
	

		if (players.Count >= 2) {
			Application.LoadLevel ("arena");
			((LevelLoader)GameObject.Find ("LevelLoader").GetComponent<LevelLoader>()).RpcLoadArena();
		}
	}

//	public override void OnServerRemovePlayer (NetworkConnection conn, UnityEngine.Networking.PlayerController player)
//	{
//		base.OnServerRemovePlayer (conn, player);
//		foreach (NetworkPlayer networkPlayer in players) {
//			if (networkPlayer.Connection == conn) {
//				players.Remove(networkPlayer);
//			}
//		}
//	}
}

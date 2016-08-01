using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using Mono.Nat;

public class ServerRow : MonoBehaviour {

  public GameHost gameHost;
  public Text name;
  public Text cur;
  public Text max;

	void FixedUpdate () {
    name.text = gameHost.ip + ":" + gameHost.port;
    cur.text = gameHost.cur_players.ToString ();
    max.text = gameHost.max_players.ToString ();
	}

  public void Join () {
    Debug.Log ("Join - " + gameHost.ip + ":" + gameHost.port);
    NetworkManager networkManager = LobbyManager.singleton;

    networkManager.networkAddress = gameHost.ip;
    networkManager.networkPort = int.Parse (gameHost.port);
    networkManager.StartClient ();
  }
}

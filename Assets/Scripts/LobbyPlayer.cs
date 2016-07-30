using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {
  [SyncVar]
  public string name;

  [SyncVar]
  public int playerNum = -1;

  [SyncVar]
  public string team;

  [SyncVar]
  public int averagePing;

  public static LobbyPlayer localPlayer;

	// Use this for initialization
	void Start () {
    if (!isLocalPlayer)
      return;
    localPlayer = this;
    name = "CPT Anonymous";
    if (PlayerPrefs.HasKey ("playerName"))
      name = PlayerPrefs.GetString ("playerName");
    CmdSetName (name);

    if (isServer)
      return;
    StartCoroutine (SendPing ());
	}

  void Awake() {
    DontDestroyOnLoad(transform.gameObject);
  }

  [Command]
  public void CmdSetName (string name) {
    Debug.Log ("Set name: " + name);
    this.name = name;
  }

  [Command]
  public void CmdSetTeam (string team) {
    this.team = team;
  }

  IEnumerator SendPing () {
    while (true) {
      byte error;
      int rtt = NetworkTransport.GetCurrentRtt (connectionToServer.hostId, connectionToServer.connectionId, out error);
      Debug.Log ("RTT: " + rtt);

      CmdSetPing (rtt);
      yield return new WaitForSeconds (1);
    }
  }

  [Command]
  public void CmdSetPing(int ping) {
    Debug.Log ("Cmd Set Ping: " + ping);
    averagePing = ping;
    RpcSetPing (ping);
  }

  [ClientRpc]
  public void RpcSetPing(int ping) {
    Debug.Log ("Rpc Set Ping: " + ping);
    averagePing = ping;
  }
}

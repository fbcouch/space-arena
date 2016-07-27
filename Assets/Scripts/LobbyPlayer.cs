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
	}

  void Awake() {
    DontDestroyOnLoad(transform.gameObject);
  }

  [Command]
  public void CmdSetName (string name) {
    Debug.Log ("Set name: " + name);
    this.name = name;
  }
}

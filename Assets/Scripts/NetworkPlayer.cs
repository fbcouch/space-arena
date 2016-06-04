using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : Player {
	NetworkConnection connection;

  [SyncVar]
  public int playerNum = -1;

	public NetworkPlayer(string playerName) : base(playerName) {
	}

	public override void Replace (GameObject ship) {
		Ship = ship;
		PlayerController playerController = (PlayerController)ship.GetComponent<PlayerController> ();
		playerController.player = this;
		playerController.isDead = false;
		NetworkServer.ReplacePlayerForConnection (connection, ship, 0);
	}

  public void Start () {
    if (!isLocalPlayer)
      return;
    name = "CPT Anonymous";
    if (PlayerPrefs.HasKey ("playerName"))
      name = PlayerPrefs.GetString ("playerName");
    CmdSetName (name);
  }

  public void Update () {
    UIPlayerSlot uiPlayerSlot = GameObject.Find ("PlayerRow (" + playerNum + ")").GetComponent <UIPlayerSlot> ();
    if (uiPlayerSlot == null)
      return;
    uiPlayerSlot.player = this;
    uiPlayerSlot.lobbyPlayer = this.gameObject.GetComponent<NetworkLobbyPlayer> ();
  }

  [Command]
  public void CmdSetName (string name) {
    Debug.Log ("Set name: " + name);
    this.name = name;
  }

	public NetworkConnection Connection {
		get {
			return this.connection;
		}
		set {
			connection = value;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {
  [SyncVar]
  public string name;

  [SyncVar]
  public int playerNum = -1;

	// Use this for initialization
	void Start () {
    if (!isLocalPlayer)
      return;
    name = "CPT Anonymous";
    if (PlayerPrefs.HasKey ("playerName"))
      name = PlayerPrefs.GetString ("playerName");
    CmdSetName (name);
	}

  void Awake() {
    DontDestroyOnLoad(transform.gameObject);
  }
	
  public void Update () {
    GameObject uiGameObject = GameObject.Find ("PlayerRow (" + playerNum + ")");
    if (uiGameObject == null)
      return;
    UIPlayerSlot uiPlayerSlot = uiGameObject.GetComponent <UIPlayerSlot> ();
    if (uiPlayerSlot == null)
      return;
    uiPlayerSlot.lobbyPlayer = this.gameObject.GetComponent<LobbyPlayer> ();
  }

  [Command]
  public void CmdSetName (string name) {
    Debug.Log ("Set name: " + name);
    this.name = name;
  }
}

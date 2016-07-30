using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerRow : MonoBehaviour {

  public string team = "blue";
  public Color blueColor = new Color (33, 150, 243);
  public Color redColor = new Color (255, 87, 34);
  public int index = 0;

  public Image background;
  public Text name;
  public Text ping;
  public Text ready;

  LobbyManager lobbyManager;

	// Use this for initialization
	void Start () {
    if (team == "blue")
      background.color = blueColor;
    if (team == "red")
      background.color = redColor;

    lobbyManager = LobbyManager.singleton as LobbyManager;
	}
	
	void FixedUpdate () {
    int i = 0;
    foreach (LobbyPlayer lobbyPlayer in lobbyManager.lobbySlots) {
      if (lobbyPlayer == null)
        continue;
      if (lobbyPlayer.team != team)
        continue;
      if (index == i) {
        GetComponent<CanvasGroup> ().alpha = 1f;
        name.text = lobbyPlayer.name;
        ping.text = lobbyPlayer.averagePing.ToString ();
        if (lobbyPlayer.readyToBegin) {
          ready.text = "Y";
        } else {
          ready.text = "";
        }
        return;
      }
      i++;
    }
    name.text = "[EMPTY]";
    GetComponent<CanvasGroup> ().alpha = 0.5f;
	}
}

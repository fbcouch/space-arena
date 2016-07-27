using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerRow : MonoBehaviour {

  public string team = "blue";
  public Color blueColor = new Color (33, 150, 243);
  public Color redColor = new Color (255, 87, 34);
  public int index = 0;

  public Color notReadyColor = new Color(1f, 1f, 1f, 0.5f);
  public Color readyColor = Color.white;

  public Image background;
  public Text name;
  public Text ping;

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
        name.text = lobbyPlayer.name;
        if (lobbyPlayer.readyToBegin) {
          name.color = readyColor;
        } else {
          name.color = notReadyColor;
        }
      }
      i++;
    }
	}
}

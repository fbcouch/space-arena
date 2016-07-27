using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReadyButton : MonoBehaviour {

  public string readyText = "READY";
  public string notReadyText = "NOT READY";

  public Text text;

	// Use this for initialization
	void Start () {
    text.text = readyText;
	}

  public void Toggle () {
    if (LobbyPlayer.localPlayer != null && text != null) {
      if (text.text == readyText) {
        LobbyPlayer.localPlayer.SendReadyToBeginMessage ();
        text.text = notReadyText;
      } else if (text.text == notReadyText) {
        LobbyPlayer.localPlayer.SendNotReadyToBeginMessage ();
        text.text = readyText;
      }
    }
  }
}

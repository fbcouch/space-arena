using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SuicideUI : MonoBehaviour {
  public Text timer, warning;

  public string warningMessage;
  	
	// Update is called once per frame
	void Update () {
    if (PlayerController.localPlayer == null)
      return;
    if (PlayerController.localPlayer.deserterCountdown != PlayerController.localPlayer.deserterTimeout) {
      timer.text = PlayerController.localPlayer.deserterCountdown.ToString ("00.000");
      warning.text = warningMessage;
    } else {
      timer.text = "";
      warning.text = "";
    }
	}
}

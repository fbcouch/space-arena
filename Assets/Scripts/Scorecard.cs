using UnityEngine;
using System.Collections;

public class Scorecard : MonoBehaviour {
  public ScorecardRow[] blueTeam;
  public ScorecardRow[] redTeam;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    int blueIndex = 0;
    int redIndex = 0;
    foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("GamePlayer")) {
      Player player = playerObject.GetComponent<Player> ();
      if (player.team == "blue") {
        if (blueIndex < blueTeam.Length)
          blueTeam [blueIndex].player = player;
        blueIndex++;
      } else if (player.team == "red") {
        if (redIndex < redTeam.Length)
          redTeam [redIndex].player = player;
        redIndex++;
      }
    }

    foreach (ScorecardRow row in blueTeam)
      row.GetComponent<CanvasGroup> ().alpha = (row.player == null ? 0 : 1);

    foreach (ScorecardRow row in redTeam)
      row.GetComponent<CanvasGroup> ().alpha = (row.player == null ? 0 : 1);

    GetComponent<Canvas> ().enabled = Input.GetButton ("Scoreboard") || (PlayerController.localPlayer != null && PlayerController.localPlayer.isDead);
	}
}

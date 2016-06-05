using UnityEngine;
using System.Collections;

public class Scorecard : MonoBehaviour {
  public ScorecardRow[] rows;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    int i = 0;
    foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("GamePlayer")) {
      Player player = playerObject.GetComponent<Player> ();
      if (i < rows.Length)
        rows [i].player = player;
      i++;
    }

    GetComponent<Canvas> ().enabled = Input.GetButton ("Scoreboard");
	}
}

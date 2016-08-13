using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
  public int direction = 1;

  GameObject[] players = new GameObject[0] {};

  public RadarIndicator indicatorPrefab;
	
	// Update is called once per frame
	void Update () {
    if (players.Length == 0) {
      players = GameObject.FindGameObjectsWithTag ("Player");

      foreach (GameObject player in players) {
        var indicator = Instantiate (indicatorPrefab) as RadarIndicator;
        indicator.playerController = player.GetComponent<PlayerController> ();
        indicator.direction = direction;
        indicator.transform.parent = transform;
      }
    }
	}
}

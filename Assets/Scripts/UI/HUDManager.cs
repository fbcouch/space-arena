using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {
  GameObject[] players;

  public HUDIndicator indicatorPrefab;

  void Start () {
    players = new GameObject[0] {};
  }
	
	// Update is called once per frame
	void Update () {
    if (players.Length == 0) {
      players = GameObject.FindGameObjectsWithTag ("Player");

      foreach (GameObject player in players) {
        var indicator = Instantiate (indicatorPrefab) as HUDIndicator;
        indicator.playerController = player.GetComponent<PlayerController> ();
        indicator.transform.parent = transform;
      }
    }
	}
}

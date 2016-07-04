using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDScore : MonoBehaviour {
  public Text redScore;
  public Text blueScore;
  public Text timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    if (GameController.instance == null)
      return;
    int min = (int)(GameController.instance.timeRemaining) / 60;
    int sec = (int)(GameController.instance.timeRemaining) % 60;
    timer.text = min + ":" + sec.ToString ("00");

    redScore.text = GameController.instance.redScore.ToString ();
    blueScore.text = GameController.instance.blueScore.ToString ();
	}
}

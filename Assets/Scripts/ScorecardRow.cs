using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorecardRow : MonoBehaviour {

  public Player player;
  public Text nameText;
  public Text killsText;
  public Text deathsText;
  public Text pingText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    if (player == null)
      return;

    nameText.text = player.name;
    if (player.isLocalPlayer)
      pingText.text = "" + Network.GetAveragePing(Network.player);

    if (player.ship == null)
      return;
    PlayerController playerController = player.ship.GetComponent<PlayerController> ();
    if (playerController == null)
      return;
    
    killsText.text = "" + playerController.kills;
    deathsText.text = "" + playerController.deaths;
	}
}

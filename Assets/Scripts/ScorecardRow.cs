using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorecardRow : MonoBehaviour {

  public Player player;
  public Text nameText;
  public Text killsText;
  public Text deathsText;
  public Text pingText;

  public Color deadColor = new Color(1f, 1f, 1f, 0.5f);
  public Color aliveColor = Color.white;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    if (player == null)
      return;

    nameText.text = player.name;
    if (player.isLocalPlayer)
      pingText.text = "" + player.averagePing;

    if (player.ship == null)
      return;
    PlayerController playerController = player.ship.GetComponent<PlayerController> ();
    if (playerController == null)
      return;
    
    killsText.text = "" + playerController.kills;
    deathsText.text = "" + playerController.deaths;

    if (playerController.isDead) {
      nameText.color = deadColor;
      killsText.color = deadColor;
      deathsText.color = deadColor;
      pingText.color = deadColor;
    } else {
      nameText.color = aliveColor;
      killsText.color = aliveColor;
      deathsText.color = aliveColor;
      pingText.color = aliveColor;
    }
	}
}

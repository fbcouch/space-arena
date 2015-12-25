using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIKillsCount : MonoBehaviour {

	public Player player;

	Text textComponent;
	
	// Use this for initialization
	void Start () {
		textComponent = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			return;
		}

		textComponent.text = "" + player.kills;
	}
}

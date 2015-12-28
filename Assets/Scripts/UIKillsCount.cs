using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIKillsCount : MonoBehaviour {

	public PlayerController player;

	Text textComponent;
	
	// Use this for initialization
	void Start () {
		textComponent = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			GameObject playerObj = GameObject.FindGameObjectWithTag("Player") as GameObject;
			if (playerObj) player = playerObj.GetComponent<PlayerController>();
			return;
		}

		textComponent.text = "" + player.kills;
	}
}

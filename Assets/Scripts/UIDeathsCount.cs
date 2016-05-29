using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDeathsCount : LocalPlayerBehaviour {
	Text textComponent;
	
	// Use this for initialization
	void Start () {
		textComponent = GetComponent<Text> ();
	}

	public override void OnUpdate () {
		if (player)
			textComponent.text = "" + player.deaths;
	}
}

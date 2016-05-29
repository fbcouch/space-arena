﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIKillsCount : LocalPlayerBehaviour {
	Text textComponent;
	
	// Use this for initialization
	void Start () {
		textComponent = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	public override void OnUpdate () {
		if (player)
			textComponent.text = "" + player.kills; 
	}
}

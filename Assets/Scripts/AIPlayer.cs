using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AIPlayer : Player {

	// Use this for initialization
	void Start () {
    serverControl = true;
	}
	
	// Update is called once per frame
	void Update () {
    if (!isServer)
      return;
	
    throttle = 1.0f;
	}
}

using UnityEngine;
using System.Collections;

public class LocalPlayerBehaviour : MonoBehaviour {
	public PlayerController player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = FindPlayerController();
			return;
		}
		OnUpdate ();
	}

	public virtual void OnUpdate () {}

	PlayerController FindPlayerController () {
		Debug.Log ("Looking for player...");
		foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag ("Player")) {
			PlayerController playerController = (PlayerController)playerObj.GetComponent<PlayerController>();
			if (playerController.isLocalPlayer) {
				Debug.Log ("Found local player!");
				return playerController;
			}
		}
		return null;
	}
}

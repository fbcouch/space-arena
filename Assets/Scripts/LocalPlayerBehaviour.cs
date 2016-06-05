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
		foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag ("Player")) {
			PlayerController playerController = (PlayerController)playerObj.GetComponent<PlayerController>();
      if (playerController && playerController.player && playerController.player.isLocalPlayer) {
				return playerController;
			}
		}
		return null;
	}
}

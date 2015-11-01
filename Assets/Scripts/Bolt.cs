using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bolt : NetworkBehaviour {
	public int damage;
	public GameObject shooter;

	void OnTriggerEnter(Collider other) {
		if (!isServer)
			return;

		if (other.gameObject == shooter)
			return;

		if (other.gameObject.tag == "Player") {
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			player.TakeDamage(damage);
			GetComponent<Exploder>().Explode();
			NetworkServer.Destroy (gameObject);
		}
	}
}

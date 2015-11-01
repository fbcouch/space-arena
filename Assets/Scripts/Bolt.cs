using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bolt : NetworkBehaviour {
	public int damage;
	public GameObject shooter;
	public GameObject explosion;

	void OnTriggerEnter(Collider other) {
		if (!isServer)
			return;

		if (other.gameObject == shooter)
			return;

		if (other.gameObject.tag == "Player") {
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			player.TakeDamage(damage);
			Rigidbody rigidbody = GetComponent<Rigidbody>();
			GameObject explosionObj = Instantiate (explosion, rigidbody.position, rigidbody.rotation) as GameObject;
			NetworkServer.Spawn (explosionObj);
			NetworkServer.Destroy (gameObject);
		}
	}
}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Exploder : NetworkBehaviour {
	public GameObject explosion;

	public void Explode() {
		Rigidbody rigidBody = GetComponent<Rigidbody> ();
		GameObject explosionObj = Instantiate (explosion, rigidBody.position, rigidBody.rotation) as GameObject;
		NetworkServer.Spawn (explosionObj);
	}
}

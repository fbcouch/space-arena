using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Mover : NetworkBehaviour {
	public float speed;
	
	void Start () {
		Rigidbody rigidBody = GetComponent<Rigidbody> ();
		
		rigidBody.velocity = transform.forward * speed;
	}
}

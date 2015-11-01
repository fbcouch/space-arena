using UnityEngine;
using UnityEngine.Networking;
using ProgressBar;
using System.Collections;

public class PlayerController : NetworkBehaviour {
	public Vector3 angularSpeed;
	public float speed;
	public GameObject shot;
	public Transform shotSpawn;
	public float shotSpeed;
	public float fireRate;
	public Camera camera;
	public int curHealth;
	public float throttle;
	private float nextFire = 0.0f;

	void Start () {
		if (!isLocalPlayer) {
			return;
		}

		AttachCamera ();
	}
	
	void FixedUpdate () {
		if (!isLocalPlayer) {
			return;
		}
		float roll = Input.GetAxis ("Horizontal");
		float pitch = Input.GetAxis ("Vertical");
		float yaw = Input.GetAxis ("Rudder");
		throttle = (Input.GetAxis ("Throttle") + 1);

		Rigidbody rigidBody = GetComponent<Rigidbody> ();
		rigidBody.velocity = rigidBody.rotation * new Vector3 (0, 0, speed * throttle);
		
		rigidBody.angularVelocity = rigidBody.rotation * new Vector3 (pitch * angularSpeed.x, -yaw * angularSpeed.y, -roll * angularSpeed.z);
	}
	
	void Update () {
	    if (!isLocalPlayer) {
			return;
		}
		
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			CmdDoFire();
		}

		GameObject throttleUI = GameObject.Find ("UIThrottle");
		throttleUI.GetComponent<ProgressRadialBehaviour> ().Value = throttle * 50;
	}

	[Command]
	void CmdDoFire () {
		GameObject missile = Instantiate (shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
		missile.GetComponent<Bolt> ().shooter = gameObject;
		Rigidbody rigidBody = missile.GetComponent<Rigidbody> ();
		
		rigidBody.velocity = transform.forward * shotSpeed;
		NetworkServer.Spawn (missile);
	}
	
	void AttachCamera () {
		camera = Camera.main;
		if (camera) {
			camera.GetComponent<FollowCamera>().target = gameObject;
		}
	}
	
	void DetachCamera () {
		if (camera) {
			camera.GetComponent<FollowCamera>().target = null;
		}
	}

	public void TakeDamage (int amount) {
		curHealth -= amount;
		Debug.Log ("Took " + amount + " damage. Current Health: " + curHealth);
		if (curHealth <= 0) {
			GetComponent<Exploder>().Explode();
			NetworkServer.Destroy (this.gameObject);
		}
	}
}

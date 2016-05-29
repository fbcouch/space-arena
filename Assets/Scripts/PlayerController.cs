using UnityEngine;
using UnityEngine.Networking;
using ProgressBar;
using System.Collections;

public class PlayerController : NetworkBehaviour {
	public Vector3 angularSpeed;
	public float speed;
	public GameObject shot;
	public Transform[] shotSpawns;
	public float shotSpeed;
	public float fireRate;
	public Camera camera;
	public int curHealth;
	public int maxHealth;
	public float throttle;
	public Texture2D boxImage;
	public Texture2D pointerImage;
	public float pointerSize = 16.0f;

	private float nextFire = 0.0f;

	public Player player;

	[SyncVar]
	public int kills = 0;
	[SyncVar]
	public int deaths = 0;
	[SyncVar]
	public bool isDead = true;
	[SyncVar]
	public string playerName;

	bool wasDead = true;

	void Start () {
		if (!isLocalPlayer) {
			return;
		}

		AttachCamera ();
	}
	
	void FixedUpdate () {
		if (!isLocalPlayer) return;
		if (isDead) return;

		float roll = Input.GetAxis ("Horizontal");
		float pitch = Input.GetAxis ("Vertical");
		float yaw = Input.GetAxis ("Rudder");
		throttle = (Input.GetAxis ("Throttle") + 1);

		Rigidbody rigidBody = GetComponent<Rigidbody> ();
		rigidBody.velocity = rigidBody.rotation * new Vector3 (0, 0, speed * throttle);
		
		rigidBody.angularVelocity = rigidBody.rotation * new Vector3 (pitch * angularSpeed.x, -yaw * angularSpeed.y, -roll * angularSpeed.z);
	}
	
	void Update () {
		if (isDead != wasDead) {
			Debug.Log ("Dead status changed");
			setRendererEnabled (!isDead);
			setColliderEnabled (!isDead);
			wasDead = isDead;
		}

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

	void OnGUI () {
		if (isLocalPlayer)
			return;
		if (isDead)
			return;
		Vector3 itemScreenPosition = Camera.main.WorldToScreenPoint (transform.position);
		float distance = Vector3.Distance (transform.position, Camera.main.transform.position) / 4;

		if (itemScreenPosition.z > 0) {
			float width = boxImage.width / distance;
			if (width < 16)
				width = 16;
			float height = boxImage.height / distance;
			if (height < 16)
				height = 16;
			float posX = itemScreenPosition.x - width / 2;
			float posY = Screen.height - itemScreenPosition.y - height / 2;
			GUIStyle style = new GUIStyle ();
			style.fontSize = (int)(24 / distance);
			if (style.fontSize < 12)
				style.fontSize = 12;
			style.alignment = TextAnchor.UpperLeft;
			style.normal.textColor = Color.white;
			GUI.contentColor = Color.white;
			GUI.color = Color.magenta;
			GUI.DrawTexture (new Rect (posX, posY, width, height), boxImage);
			GUI.Label (new Rect (posX, posY - style.fontSize * 1.25f, width, height), "[" + kills + "/" + deaths + "] " + playerName, style);
		}
		Rect screenRect = new Rect (0, 0, Screen.width, Screen.height);
		if (itemScreenPosition.z <= 0 || !screenRect.Contains(new Vector2(itemScreenPosition.x, itemScreenPosition.y))) {
			float mul = itemScreenPosition.z / Mathf.Abs (itemScreenPosition.z);
			Vector2 location = new Vector2 (itemScreenPosition.x - Screen.width / 2, itemScreenPosition.y - Screen.height / 2);
			location = location / location.magnitude;
			location = location * 256 * mul;

			GUI.color = Color.magenta;
			GUI.DrawTexture (new Rect (Screen.width / 2 + location.x - pointerSize / 2, Screen.height / 2 - location.y - pointerSize / 2, pointerSize, pointerSize), pointerImage);
			GUI.color = Color.green;
			GUI.DrawTexture (new Rect (itemScreenPosition.x - 8, Screen.height - itemScreenPosition.y - 8, 16, 16), pointerImage);	
		}

	}

	[Command]
	void CmdDoFire () {
		if (isDead) return;

		foreach (Transform shotSpawn in shotSpawns) {
			GameObject missile = Instantiate (shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			missile.GetComponent<Bolt> ().shooter = gameObject;
			Rigidbody rigidBody = missile.GetComponent<Rigidbody> ();
		
			rigidBody.velocity = transform.forward * shotSpeed;
			NetworkServer.Spawn (missile);
		}
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

	public void OnRespawn () {
		setRendererEnabled (true);
		setColliderEnabled (true);
		isDead = false;
		curHealth = maxHealth;
		Rigidbody rigidBody = GetComponent<Rigidbody> ();
		rigidBody.velocity = rigidBody.rotation * new Vector3 (0, 0, 0);
		
		rigidBody.angularVelocity = rigidBody.rotation * new Vector3 (0, 0, 0);
	}

	public void TakeDamage (int amount, GameObject shooter) {
		if (isDead)
			return;
		curHealth -= amount;
		Debug.Log ("Took " + amount + " damage. Current Health: " + curHealth);
		if (curHealth <= 0) {
			GetComponent<Exploder>().Explode();
//			NetworkServer.Destroy (this.gameObject);
			isDead = true;
			deaths += 1;
			((PlayerController)shooter.GetComponent<PlayerController> ()).kills += 1;
			setRendererEnabled (false);
			setColliderEnabled (false);
		}
	}

	void setRendererEnabled(bool enabled) {
		foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>()) {
			r.enabled = enabled;
		}
	}

	void setColliderEnabled(bool enabled) {
		foreach (Collider r in gameObject.GetComponentsInChildren<Collider>()) {
			r.enabled = enabled;
		}
	}
}

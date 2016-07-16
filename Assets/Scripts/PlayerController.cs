﻿using UnityEngine;
using UnityEngine.Networking;
using ProgressBar;
using System.Collections;

public class PlayerController : NetworkBehaviour {
  public static PlayerController localPlayer;

  public GameObject shot;
  public Transform[] shotSpawns;
  public Camera camera;
  public GameObject shooter;

  public Texture2D boxImage;
  public Texture2D leadTargetImage;
  public float leadTargetSize = 12.0f;
  public Texture2D pointerImage;
  public float pointerSize = 16.0f;
  public Texture2D minimapImage;
  public Texture2D minimapPointerImage;
  public float minimapPointerSize = 6.0f;
  public Texture2D reticuleImage;
  public Color reticuleColor = Color.white;
  public Vector2 reticuleOffset = new Vector2(0, 0);

  public Vector3 angularThrust;
  public Vector3 maxAngularSpeed;
  public float thrust;
  public float maxSpeed;
  [SyncVar]
  public int curHealth = 20;
  [SyncVar]
  public int maxHealth = 20;
  [SyncVar]
  public int maxShield = 10;
  [SyncVar]
  public int curShield = 10;
  [SyncVar]
  public int maxAmmo = 8;
  [SyncVar]
  public int curAmmo = 8;
  public float shotSpeed;
  public float fireRate;
  public float shieldRate = 1;
  public float ammoRate = 0.5f;

  public int deserterTimeout = 15;
  public int deserterRadius = 5000;
  public float deserterCountdown;

  private float nextShield = 0.0f;
  private float nextAmmo = 0.0f;

  private float nextFire = 0.0f;
  private GameObject target;

  private bool fire2Up = false;

  public Player player;

  [SyncVar]
  public int kills = 0;
  [SyncVar]
  public int deaths = 0;
  [SyncVar]
  public bool isDead = true;
  [SyncVar]
  public string playerName;
  [SyncVar]
  public int playerNum = -1;

  bool wasDead = true;

  private UIPercentPills throttlePills;
  private UIPercentPills weaponPills;
  private UIPercentPills hullPills;
  private UIPercentPills shieldPills;
  private GameObject centerHUD;

  void Start () {
    if (player == null) {
      foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("GamePlayer")) {
        Player gamePlayer = gameObject.GetComponent<Player> ();
        if (gamePlayer != null && gamePlayer.playerNum == playerNum) {
          player = gamePlayer;
          player.Ship = this.gameObject;
        }
      }
    }

    if (!(player && player.isLocalPlayer)) {
      return;
    }

    localPlayer = this;

    AttachCamera ();
    throttlePills = GameObject.Find ("ThrottlePills").GetComponent<UIPercentPills> ();
    weaponPills = GameObject.Find ("WeaponPills").GetComponent<UIPercentPills> ();
    hullPills = GameObject.Find ("HullPills").GetComponent<UIPercentPills> ();
    shieldPills = GameObject.Find ("ShieldPills").GetComponent<UIPercentPills> ();
    centerHUD = GameObject.Find ("HUDCenter");
  }

  private Vector3 dampVelocity = Vector3.zero;
  private Vector3 dampAngularVelocity = Vector3.zero;

  void FixedUpdate () {
    if (isDead) return;
    if (!GameController.instance.IsGameRunning ()) return;
    Rigidbody rigidBody = GetComponent<Rigidbody> ();

    if (!player)
      return;

    if (!(player.isLocalPlayer || (isServer && player.serverControl)))
      return;

    // Clamp speed
    float targetSpeed = player.throttle * maxSpeed;
    if (rigidBody.velocity.sqrMagnitude > Mathf.Pow(targetSpeed, 2)) {
      rigidBody.AddForce(rigidBody.velocity / rigidBody.velocity.magnitude * -1 * thrust);
    } else {
      rigidBody.AddForce(transform.forward * thrust * player.throttle);
    }
    rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxSpeed);
    if (rigidBody.velocity.sqrMagnitude < 1 && player.throttle < 0.01f)
      rigidBody.velocity = Vector3.zero;

    rigidBody.AddTorque (rigidBody.rotation * new Vector3 (player.pitch * angularThrust.x, -player.yaw * angularThrust.y, -player.roll * angularThrust.z));
    // Clamp rotation
    Vector3 rotatedSpeeds = Quaternion.Inverse(transform.rotation) * rigidBody.angularVelocity;
    if (rotatedSpeeds.x > maxAngularSpeed.x)
      rotatedSpeeds.x = maxAngularSpeed.x;
    if (rotatedSpeeds.y > maxAngularSpeed.y)
      rotatedSpeeds.y = maxAngularSpeed.y;
    if (rotatedSpeeds.z > maxAngularSpeed.z)
      rotatedSpeeds.z = maxAngularSpeed.z;
    rigidBody.angularVelocity = transform.rotation * rotatedSpeeds;
    rigidBody.angularVelocity = Vector3.SmoothDamp(rigidBody.angularVelocity, new Vector3(0, 0, 0), ref dampAngularVelocity, 0.25f);
  }

  void Update () {
    if (isDead != wasDead) {
      Debug.Log ("Dead status changed");
      setRendererEnabled (!(player && player.isLocalPlayer) && !isDead);
      setColliderEnabled (!isDead);
      wasDead = isDead;
    }

    if (centerHUD != null)
      centerHUD.SetActive (!isDead);

    if (isDead)
      return;

    if (Time.time > nextShield) {
      curShield = Mathf.Clamp (curShield + 1, 0, maxShield);
      nextShield = Time.time + shieldRate;
    }

    if (!player)
      return;

    if (!(player.isLocalPlayer || (isServer && player.serverControl)))
      return;

    if (GameController.instance.IsGameRunning () && transform.position.sqrMagnitude > deserterRadius * deserterRadius) {
      deserterCountdown -= Time.deltaTime;
      if (deserterCountdown <= 0) {
        CmdSuicide ();
      }
    } else {
      deserterCountdown = deserterTimeout;
    }

    if (player.fire1 && Time.time > nextFire && curAmmo >= shotSpawns.Length) {
      Debug.Log ("Fire!!!");
      nextFire = Time.time + fireRate;
      player.FireWeapons();
      curAmmo -= shotSpawns.Length;
    }

    if (player.fire2 && fire2Up) {
      TargetAhead ();
      fire2Up = false;
    } else {
      fire2Up = true;
    }

    if (Time.time > nextAmmo) {
      curAmmo = Mathf.Clamp (curAmmo + 1, 0, maxAmmo);
      nextAmmo = Time.time + ammoRate;
    }

    if (!player.isLocalPlayer)
      return;

    throttlePills.max = 1;
    throttlePills.current = player.throttle;

    hullPills.max = maxHealth;
    hullPills.current = curHealth;

    weaponPills.max = maxAmmo;
    weaponPills.current = curAmmo;

    shieldPills.max = maxShield;
    shieldPills.current = curShield;
  }

  void TargetAhead () {
    float minDist = -1;
    GameObject newTarget = null;
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag ("Player")) {
      if (gameObject == this.gameObject)
        continue;
      var playerController = gameObject.GetComponent<PlayerController> ();
      if (playerController == null || playerController.isDead)
        continue;
      Vector3 itemScreenPosition = Camera.main.WorldToScreenPoint (gameObject.transform.position);
      float distance = (gameObject.transform.position - Camera.main.transform.position).sqrMagnitude;
      if (itemScreenPosition.z >= 0 && new Vector2 (Screen.width / 2 - itemScreenPosition.x, Screen.height / 2 - itemScreenPosition.y).sqrMagnitude <= Mathf.Pow(reticuleImage.width / 2, 2)) {
        if (newTarget == null || distance < minDist) {
          newTarget = gameObject;
          minDist = distance;
        }
      }
    }
    target = newTarget;
  }

  void DrawReticule () {
    GUI.color = reticuleColor;
    GUI.DrawTexture (new Rect ((Screen.width - reticuleImage.width) / 2 + reticuleOffset.x, (Screen.height - reticuleImage.height) / 2 + reticuleOffset.y, reticuleImage.width, reticuleImage.height), reticuleImage);
  }

  void DrawMinimap () {
    GUI.color = Color.white;
    GUI.DrawTexture (new Rect (0, Screen.height - minimapImage.height, minimapImage.width, minimapImage.height), minimapImage);
  }

  void DrawStats () {
    Rigidbody rigidBody = GetComponent<Rigidbody> ();
    GUIStyle style = new GUIStyle ();
    style.normal.textColor = Color.white;
    style.alignment = TextAnchor.LowerRight;
    GUI.Label (new Rect (0, Screen.height - 50, Screen.width, 50), "Velocity: " + Mathf.Round(rigidBody.velocity.magnitude) + "m/s", style);
    GUI.Label (new Rect (0, Screen.height - 64, Screen.width, 50), "Hull: " + Mathf.Round(100.0f * curHealth / maxHealth) + "%", style);
  }

  void DrawHUDBox (GameObject gameObject, PlayerController playerController) {
    Vector3 itemScreenPosition = Camera.main.WorldToScreenPoint (gameObject.transform.position);
    float distance = Vector3.Distance (gameObject.transform.position, Camera.main.transform.position) / 4;

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
      style.alignment = TextAnchor.UpperCenter;
      style.normal.textColor = Color.white;
      GUI.color = colorForGameObject(gameObject);
      GUI.DrawTexture (new Rect (posX, posY, width, height), boxImage);
      GUI.Label (new Rect (posX, posY - style.fontSize * 1.25f, width, height), "[" + playerController.kills + "/" + playerController.deaths + "] " + playerController.playerName, style);
    }
  }

  Color colorForGameObject (GameObject gameObject) {
    if (gameObject == target) {
      return Color.magenta;
    } else {
      return Color.cyan;
    }
  }

  void DrawHUDPointer (GameObject gameObject, PlayerController playerController) {
    Vector3 itemScreenPosition = Camera.main.WorldToScreenPoint (gameObject.transform.position);
    Rect screenRect = new Rect (0, 0, Screen.width, Screen.height);
    if (itemScreenPosition.z <= 0 || !screenRect.Contains(new Vector2(itemScreenPosition.x, itemScreenPosition.y))) {
      float mul = itemScreenPosition.z / Mathf.Abs (itemScreenPosition.z);
      Vector2 location = new Vector2 (itemScreenPosition.x - Screen.width / 2, itemScreenPosition.y - Screen.height / 2);
      location = location / location.magnitude;
      location = location * 256 * mul;

      GUI.color = colorForGameObject(gameObject);
      GUI.DrawTexture (new Rect (Screen.width / 2 + location.x - pointerSize / 2, Screen.height / 2 - location.y - pointerSize / 2, pointerSize, pointerSize), pointerImage);
    }
  }

  void DrawHUDMinimap (GameObject gameObject, PlayerController playerController) {
    Vector3 heading = gameObject.transform.position - Camera.main.transform.position;
    Vector3 final = Quaternion.Inverse(Camera.main.transform.rotation) * heading;

    Vector2 loc = new Vector2(final.x, final.z);
    loc = Vector3.ClampMagnitude(loc, 2000);
    loc = loc / 2000 * 120;
    GUI.color = colorForGameObject(gameObject);
    GUI.DrawTexture (new Rect (128 + loc.x - minimapPointerSize / 2, Screen.height - 128 - loc.y - minimapPointerSize / 2, minimapPointerSize, minimapPointerSize), minimapPointerImage);
  }

  void DrawLeadIndicator (GameObject gameObject, PlayerController playerController) {
    float distance = Vector3.Distance(transform.position, gameObject.transform.position);
    float time = distance / shotSpeed;
    Vector3 velocity = gameObject.GetComponent<Rigidbody> ().velocity;
    Vector3 newPosition = gameObject.transform.position + velocity * time;

    Vector3 itemScreenPosition = Camera.main.WorldToScreenPoint (newPosition);

    if (itemScreenPosition.z > 0) {
      float posX = itemScreenPosition.x - leadTargetSize / 2;
      float posY = Screen.height - itemScreenPosition.y - leadTargetSize / 2;
      GUIStyle style = new GUIStyle ();
      if (distance < shotSpeed) {
        GUI.color = style.normal.textColor = Color.white;
      } else {
        GUI.color = style.normal.textColor = new Color(255f, 255f, 255f, 0.75f);
      }
      GUI.DrawTexture (new Rect (posX, posY, leadTargetSize, leadTargetSize), leadTargetImage);

      style.alignment = TextAnchor.LowerCenter;
      GUI.Label (new Rect (itemScreenPosition.x - 50, Screen.height - itemScreenPosition.y + leadTargetSize, 100, 10f), Mathf.Round(distance) + "m", style);
    }
  }

  void OnGUI () {
    if (!(player && player.isLocalPlayer))
      return;
    if (isDead)
      return;
    DrawReticule ();
    DrawMinimap ();
    DrawStats ();

    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag ("Player")) {
      PlayerController otherController = gameObject.GetComponent<PlayerController> ();
      if (this == otherController)
        continue;
      if (otherController == null || otherController.isDead)
        continue;

      DrawHUDBox (gameObject, otherController);
      DrawHUDPointer (gameObject, otherController);
      DrawHUDMinimap (gameObject, otherController);
      if (target == gameObject)
        DrawLeadIndicator (gameObject, otherController);
    }
  }
 
  public void FireWeapons () {
    if (isDead) return;

    foreach (Transform shotSpawn in shotSpawns) {
      GameObject missile = Instantiate (shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
      missile.GetComponent<Bolt> ().shooter = shooter.gameObject;
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

  public void OnRespawn (GameObject spawn) {
    setRendererEnabled (true);
    setColliderEnabled (true);
    isDead = false;
    curHealth = maxHealth;
    curShield = maxShield;
    curAmmo = maxAmmo;
    Rigidbody rigidBody = GetComponent<Rigidbody> ();
    rigidBody.velocity = rigidBody.rotation * new Vector3 (0, 0, 0);

    rigidBody.angularVelocity = rigidBody.rotation * new Vector3 (0, 0, 0);
    RpcOnRespawn (spawn.transform.position, spawn.transform.rotation);
  }

  [ClientRpc]
  public void RpcOnRespawn(Vector3 position, Quaternion rotation) {
    transform.position = position;
    transform.rotation = rotation;
  }

  [Command]
  public void CmdSuicide () {
    Debug.Log ("Suicide");
    isDead = true;
    deaths += 1;
    setRendererEnabled (false);
    setColliderEnabled (false);
  }

  public void TakeDamage (int amount, GameObject shooter) {
    if (isDead)
      return;
    if (curShield > 0) {
      curShield -= amount;
      if (curShield < 0) {
        curHealth += curShield;
        curShield = 0;
      }
    } else {
      curHealth -= amount;
    }
    Debug.Log ("Took " + amount + " damage. Current Health: " + curHealth + ", Current Shield: " + curShield);
    if (curHealth <= 0) {
      GetComponent<Exploder>().Explode();
//			NetworkServer.Destroy (this.gameObject);
      isDead = true;
      deaths += 1;
      Rigidbody rigidBody = GetComponent<Rigidbody> ();
      rigidBody.velocity = rigidBody.rotation * new Vector3 (0, 0, 0);
      rigidBody.angularVelocity = rigidBody.rotation * new Vector3 (0, 0, 0);
      ((PlayerController)shooter.transform.parent.gameObject.GetComponent<PlayerController> ()).kills += 1;
      setRendererEnabled (false);
      setColliderEnabled (false);
    }
  }

  void setRendererEnabled(bool enabled) {
    foreach (Renderer r in shooter.GetComponentsInChildren<Renderer>()) {
      r.enabled = enabled;
    }
  }

  void setColliderEnabled(bool enabled) {
    foreach (Collider r in shooter.GetComponentsInChildren<Collider>()) {
      r.enabled = enabled;
    }
  }
}

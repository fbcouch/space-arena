using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AIPlayer : Player {

  GameObject target;
  public float targetOffsetRadius = 20.0f;
  public float maxFireDistance = 1000f;
  public float fireTolerance = 5.0f;
  public float retargetChance = 0.001f;


	// Use this for initialization
	void Start () {
    serverControl = true;
	}
	
	// Update is called once per frame
	void Update () {
    if (!isServer)
      return;

    if (!GameController.instance.IsGameRunning ())
      return;

    if (ship == null)
      return;

    if (target == null || Random.value < retargetChance) {
      FindTarget ();
      return;
    }

    FaceTarget ();
	}

  void FindTarget () {
    float minDistSq = -1;
    GameObject minDistObj = null;
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag ("GamePlayer")) {
      Player player = gameObject.GetComponent<Player> ();
      if (player == null || player.team == team ||  player.ship == null || player.ship.GetComponent<PlayerController> ().isDead)
        continue;

      float curDistSq = (ship.transform.position - player.ship.transform.position).sqrMagnitude;
      if (minDistSq == -1 || curDistSq < minDistSq)
        minDistObj = gameObject;
    }

    target = minDistObj;
  }

  void FaceTarget () {
    Player player = target.GetComponent<Player> ();
    if (player.ship.GetComponent<PlayerController> ().isDead) {
      target = null;
      return;
    }
    Vector3 toTarget = player.ship.transform.position - ship.transform.position;
    float distance = toTarget.magnitude;
    toTarget += player.ship.GetComponent <Rigidbody> ().velocity * distance / ship.GetComponent<PlayerController> ().shotSpeed;
    toTarget += Random.insideUnitSphere * targetOffsetRadius;

    toTarget = Quaternion.Inverse(ship.transform.rotation) * toTarget;

    pitch = -Mathf.Clamp(toTarget.y, -1f, 1f);
    yaw = -Mathf.Clamp(toTarget.x, -1f, 1f);

    throttle = Mathf.Clamp(distance / 1000f, 0.33f, 1.0f);

    fire1 = (distance < maxFireDistance && Mathf.Abs (toTarget.x) < fireTolerance && Mathf.Abs (toTarget.y) < fireTolerance && toTarget.z > 0);
  }

  public override void FireWeapons ()
  {
    if (isServer)
      CmdFireWeapons (GameController.instance.timeRemaining);
  }
}

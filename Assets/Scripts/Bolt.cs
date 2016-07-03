using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bolt : NetworkBehaviour {
  public int damage;
  public GameObject shooter;

  void OnTriggerEnter(Collider other) {
    if (!isServer)
      return;

    Debug.Log ("Other: " + other.gameObject + " Shooter: " + shooter);

    if (other.gameObject == shooter)
      return;

    GetComponent<Exploder>().Explode();
    NetworkServer.Destroy (gameObject);

    if (other.gameObject.transform.parent == null)
      return;

    GameObject otherPlayerObj = other.gameObject.transform.parent.gameObject;
    if (otherPlayerObj.tag == "Player") {
      PlayerController player = otherPlayerObj.GetComponent<PlayerController>();
      if (player.isDead) return;
      player.TakeDamage(damage, shooter);
    }
  }

  void Start() {
    if (isServer)
      return;

    setRendererEnabled (false);

    Debug.Log ("Bolt Rotation: " + gameObject.transform.rotation);
  }

  void Update() {
    if (isServer)
      return;

    setRendererEnabled (true);

    Debug.Log ("Bolt Rotation: " + gameObject.transform.rotation);
  }

  void setRendererEnabled(bool enabled) {
    foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>()) {
      r.enabled = enabled;
    }
  }
}

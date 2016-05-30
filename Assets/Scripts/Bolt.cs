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

    GetComponent<Exploder>().Explode();
    NetworkServer.Destroy (gameObject);
    if (other.gameObject.tag == "Player") {
      PlayerController player = other.gameObject.GetComponent<PlayerController>();
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

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bolt : NetworkBehaviour {
  public int damage;
  public GameObject shooter;

  void OnTriggerEnter(Collider other) {
    Debug.Log ("Other: " + other.gameObject + " Shooter: " + shooter);

    if (other.gameObject == shooter)
      return;

    GetComponent<Exploder>().Explode();
    Destroy (gameObject);

    if (other.gameObject.transform.parent == null)
      return;

    GameObject otherPlayerObj = other.gameObject.transform.parent.gameObject;
    if (otherPlayerObj.tag == "Player") {
      PlayerController player = otherPlayerObj.GetComponent<PlayerController>();
      if (player.isDead) return;
      player.TakeDamage(damage, shooter);
    }
  }
    
  void setRendererEnabled(bool enabled) {
    foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>()) {
      r.enabled = enabled;
    }
  }
}

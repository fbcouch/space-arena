using UnityEngine;
using System.Collections;

public class RateByVelocity : MonoBehaviour {
  public int scalingFactor = 3;
  ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
    particleSystem = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
    ParticleSystem.EmissionModule em = particleSystem.emission;

    if (PlayerController.localPlayer != null) {
      em.rate = new UnityEngine.ParticleSystem.MinMaxCurve (scalingFactor * PlayerController.localPlayer.GetComponent <Rigidbody> ().velocity.magnitude);
    } else {
      em.rate = new UnityEngine.ParticleSystem.MinMaxCurve (0f);
    }
	}
}

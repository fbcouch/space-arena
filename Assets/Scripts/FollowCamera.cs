using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public float interpolationVelocity;
	public float minDistance;
	public float followDistance;
	public GameObject target;
	public Vector3 offset;
	Vector3 targetPos;
	
	// Use this for initialization
	void Start () {
		targetPos = transform.position;	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target) {
			Vector3 posNoZ = transform.position;
			posNoZ.z = target.transform.position.z;
			
			Vector3 targetDirection = target.transform.position - posNoZ;
			
			interpolationVelocity = targetDirection.magnitude * 5f;
			
			targetPos = target.transform.position + (target.transform.rotation * offset);

			transform.position = Vector3.Lerp (transform.position, targetPos, 1f);

			transform.rotation = target.transform.rotation;
		}
	}
}

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
  public bool firstPersonMode = true;

  bool toggleKeyDown = false;
  bool switchKeyDown = false;
  int specIndex = 0;

  // Use this for initialization
  void Start () {
    targetPos = transform.position;
  }

  void FixedUpdate () {
    bool buttonState = Input.GetButton ("toggleCameraMode");
    if (buttonState && !toggleKeyDown) {
      firstPersonMode = !firstPersonMode;
    } 
    toggleKeyDown = buttonState;

    if (GameController.instance != null) {
      if (PlayerController.localPlayer != null && !PlayerController.localPlayer.isDead) {
        AttachCamera ();
      } else {
        SwitchTarget ();
      }
    }
  }

  void SwitchTarget () {
    bool buttonState = Input.GetButton ("Fire1");
    if (buttonState && !switchKeyDown) {
      GameObject lastGameObject = null;
      GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Player");
      specIndex = (specIndex + 1) % gameObjects.Length;
      target = gameObjects [specIndex];
    }

    switchKeyDown = buttonState;
  }

  // Update is called once per frame
  void Update () {
    if (target) {
      Vector3 posNoZ = transform.position;
      posNoZ.z = target.transform.position.z;

      Vector3 targetDirection = target.transform.position - posNoZ;

      interpolationVelocity = targetDirection.magnitude * 5f;

      if (firstPersonMode) {
        targetPos = target.transform.position;

        transform.position = targetPos;
        transform.rotation = target.transform.rotation;

        foreach (Renderer r in target.gameObject.GetComponentsInChildren<Renderer> ()) {
          r.enabled = false;
        }
      } else {
        Transform targetTransform = target.transform.FindChild ("Camera3").transform;
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;

        foreach (Renderer r in target.gameObject.GetComponentsInChildren<Renderer> ()) {
          r.enabled = true;
        }
      }
    }
  }

  public void AttachCamera () {
    target = PlayerController.localPlayer.gameObject;
  }
}

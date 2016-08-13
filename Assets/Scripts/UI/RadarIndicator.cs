using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadarIndicator : MonoBehaviour {
  public PlayerController playerController;
  public Color blueColor;
  public Color redColor;
  public Image image, targetImage;
  public int direction = 1;

  CanvasGroup cg;

	// Use this for initialization
	void Start () {
    cg = GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
    if (playerController == null || playerController.isDead || playerController == PlayerController.localPlayer) {
      cg.alpha = 0;
      return;
    }

    cg.alpha = 1;
    if (playerController.player.team == "blue") {
      image.color = blueColor;
    } else {
      image.color = redColor;
    }

    targetImage.enabled = PlayerController.localPlayer.target == playerController.gameObject;

    Vector3 heading = playerController.gameObject.transform.position - Camera.main.transform.position;
    Vector3 final = Quaternion.Inverse(Camera.main.transform.rotation) * heading;

    if ((direction > 0 && final.z > 0) || (direction < 0 && final.z < 0)) {
      float x = Mathf.Asin (final.x / (new Vector2 (final.x, direction * final.z).magnitude)) * 128 / Mathf.PI; // Mathf.Atan2 (final.x, direction * final.z) * 100 / Mathf.PI;
      float y = Mathf.Asin (final.y / (new Vector2 (final.y, direction * final.z).magnitude)) * 128 / Mathf.PI; // Mathf.Atan2 (final.y, direction * final.z) * 100 / Mathf.PI;

      var vector = new Vector3 (x, y, 0);
      if (vector.sqrMagnitude >= Mathf.Pow (58, 2))
        vector = vector.normalized * 58;
      transform.localPosition = vector;
    } else {
      cg.alpha = 0;
    }
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDIndicator : MonoBehaviour {

  public PlayerController playerController;
  public Color blueColor;
  public Color redColor;

  public Image outerImage, innerImage, middleImage;
  public Text playerName, playerDistance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
    CanvasGroup cg = GetComponent<CanvasGroup>();
    if (playerController == null || playerController.isDead) {
      cg.alpha = 0;
      return;
    }
    if (playerController == PlayerController.localPlayer) {
      cg.alpha = 0;
      return;
    }
    middleImage.color = playerName.color = playerDistance.color = Color.white;

    if (playerController.player.team == "blue") {
      outerImage.color = innerImage.color = blueColor;
    } else {
      outerImage.color = innerImage.color = redColor;
    }
      
    middleImage.enabled = innerImage.enabled = playerName.enabled = playerDistance.enabled = PlayerController.localPlayer.target == playerController.gameObject;
    playerName.text = playerController.player.name;
    float distance = Vector3.Distance(PlayerController.localPlayer.gameObject.transform.position, playerController.gameObject.transform.position);
    playerDistance.text = Mathf.Round (distance).ToString () + "m";

    cg.alpha = 1;

    Vector3 itemScreenPosition = Camera.main.WorldToScreenPoint (playerController.gameObject.transform.position);
    Rect screenRect = new Rect (0, 0, Screen.width, Screen.height);

    if (itemScreenPosition.z >= 0 || !screenRect.Contains (new Vector2 (itemScreenPosition.x, itemScreenPosition.y))) {
      transform.position = new Vector3 (itemScreenPosition.x, itemScreenPosition.y, 0);
      cg.alpha = 1;
      var rt = GetComponent<RectTransform> ();
      var size = Mathf.Clamp (Mathf.Pow (1000 / itemScreenPosition.z, 2), 0.5f, 1);
    } else {
      cg.alpha = 0;
    }
  }
}

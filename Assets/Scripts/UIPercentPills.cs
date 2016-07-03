using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPercentPills : MonoBehaviour {
  public float current = 0f;
  public float max = 0f;

	// Update is called once per frame
	void Update () {
    var image = GetComponent<RawImage> ();
    float pct = 0;
    if (max != 0)
      pct = current / max;
    pct = Mathf.Clamp (pct, 0, 1);
    var rect = new Rect (image.uvRect);
    rect.height = pct;
    image.uvRect = rect;
    image.SetNativeSize ();
	}
}

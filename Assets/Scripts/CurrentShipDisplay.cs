using UnityEngine;
using System.Collections;

public class CurrentShipDisplay : MonoBehaviour {
  public float velocity = 1.0f;

  GameObject ship;

	// Use this for initialization
	void Start () {
    ship = transform.Find ("Ship").gameObject;
  }

  void FixedUpdate () {
    transform.Rotate (new Vector3 (0, velocity * Time.deltaTime, 0));
    LoadShipFromPreferences ();
  }

  public void LoadShipFromPreferences () {
    if (!PlayerPrefs.HasKey ("shipIdentifier"))
      return;

    ShipData data = ShipDataHolder.instance.Find (PlayerPrefs.GetString ("shipIdentifier"));
    if (data == null)
      return;

    ship.GetComponent<MeshFilter> ().mesh = data.mesh;
  }
}

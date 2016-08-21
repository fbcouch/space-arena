using UnityEngine;
using System.Collections;

public class ShipDataHolder : MonoBehaviour {
  public ShipData[] shipData;

  public static ShipDataHolder instance;

	// Use this for initialization
	void Start () {
    instance = this;
	}

  void Awake () {
    DontDestroyOnLoad (this);
  }

  public ShipData Find (string identifier) {
    foreach (ShipData data in shipData) {
      if (identifier == data.identifier)
        return data;
    }
    return null;
  }
}

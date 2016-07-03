using UnityEngine;
using System.Collections;

public class TeamMaterial : MonoBehaviour {
  public Material blueMaterial;
  public Material redMaterial;
  public Material neutralMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    var playerController = GetComponentInParent <PlayerController> ();
    var mats = gameObject.GetComponent<MeshRenderer> ().materials;
    if (playerController == null || playerController.player == null || playerController.player.team == null) {
      mats [0] = neutralMaterial;
      gameObject.GetComponent<MeshRenderer> ().materials = mats;
      return;
    }

    if (playerController.player.team == "blue") {
      if (mats [0] == blueMaterial)
        return;
      mats [0] = blueMaterial;
    }

    if (playerController.player.team == "red") {
      if (mats [0] == redMaterial)
        return;
      mats [0] = redMaterial;
    }
    gameObject.GetComponent<MeshRenderer> ().materials = mats;
	}
}

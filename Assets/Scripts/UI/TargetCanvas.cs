using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetCanvas : MonoBehaviour {
  public UIPercentPills hullPills, shieldPills;
  public Text targetName, targetDist, targetKills, targetDeaths;
  public MeshRenderer targetRenderer;
  public MeshFilter targetFilter;
  public Material blueMaterial, redMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    if (PlayerController.localPlayer == null || PlayerController.localPlayer.target == null) {
      hullPills.current = 0;
      shieldPills.current = 0;
      targetName.text = targetDist.text = targetKills.text = targetDeaths.text = "";
      targetFilter.mesh = null;
      return;
    }

    var playerController = PlayerController.localPlayer.target.GetComponent<PlayerController> ();
    hullPills.max = playerController.maxHealth;
    hullPills.current = playerController.curHealth;
    shieldPills.max = playerController.maxShield;
    shieldPills.current = playerController.curShield;

    targetName.text = playerController.playerName;
    targetKills.text = "Kills: " + playerController.kills.ToString ();
    targetDeaths.text = "Deaths: " + playerController.deaths.ToString ();

    float distance = Vector3.Distance(PlayerController.localPlayer.gameObject.transform.position, playerController.gameObject.transform.position);
    targetDist.text = Mathf.Round (distance).ToString () + "m";

    MeshRenderer renderer = PlayerController.localPlayer.target.GetComponentInChildren<MeshRenderer> ();
    MeshFilter filter = PlayerController.localPlayer.target.GetComponentInChildren<MeshFilter> ();
    if (playerController.player.team == "blue") {
      targetRenderer.materials = new Material[] { blueMaterial, blueMaterial };
    } else {
      targetRenderer.materials = new Material[] { redMaterial, redMaterial };
    }
    targetFilter.mesh = filter.mesh;
	}
}

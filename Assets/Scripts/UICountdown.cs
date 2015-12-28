using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICountdown : MonoBehaviour {
	public GameController gameController;

	Text textComponent;
	
	// Use this for initialization
	void Start () {
		textComponent = GetComponent<Text> ();
	}

	
	// Update is called once per frame
	void Update () {
		if (!gameController) {
			GameObject controller = GameObject.FindGameObjectWithTag("GameController");
			if (controller)
				gameController = controller.GetComponent<GameController>();
			return;
		}

		if (gameController.countdown > 0) {
			textComponent.text = "" + gameController.countdown;
		} else {
			textComponent.text = "";
		}
	}
}

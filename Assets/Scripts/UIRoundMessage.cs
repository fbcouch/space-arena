using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRoundMessage : MonoBehaviour {
	public GameController gameController;
	public string preGameStart;
	public string gameStarting;
	public string gameInProgress;
	public string roundInProgress;
	public string roundStarting;
	public string roundEnding;
  public string gameOver;

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

    if (gameController.gameStarting) {
      textComponent.text = gameStarting;
    } else if (gameController.gameStarted) {
      if (gameController.roundStarting) {
        textComponent.text = roundStarting;
      } else if (gameController.roundStarted) {
        textComponent.text = roundInProgress;
      } else if (gameController.roundEnding) {
        textComponent.text = roundEnding;
      } else {
        textComponent.text = gameInProgress;
      }
    } else if (gameController.gameOver) {
      textComponent.text = gameOver;
		} else {
			textComponent.text = preGameStart;
		}
	}
}

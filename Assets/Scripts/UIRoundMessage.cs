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
  public string overtimeStarting;

  public string redTeamWins, blueTeamWins, tie;

	public Text textRoundMessage;
  public Text textGameOverMessage;
	
	// Update is called once per frame
	void Update () {
    if (gameController == null)
      gameController = GameController.instance;
    
    if (gameController.gameStarting) {
      textRoundMessage.text = gameStarting;
    } else if (gameController.gameStarted) {
      if (gameController.roundStarting) {
        if (gameController.overtime) {
          textRoundMessage.text = overtimeStarting;
        } else {
          textRoundMessage.text = roundStarting;
        }
      } else if (gameController.roundStarted) {
        textRoundMessage.text = roundInProgress;
      } else if (gameController.roundEnding) {
        textRoundMessage.text = roundEnding;
      } else {
        textRoundMessage.text = gameInProgress;
      }
    } else if (gameController.gameOver) {
      textRoundMessage.text = gameOver;
		} else {
			textRoundMessage.text = preGameStart;
		}

    if (gameController.gameOver) {
      if (gameController.blueScore > gameController.redScore) {
        textGameOverMessage.text = blueTeamWins;
      } else if (gameController.redScore > gameController.blueScore) {
        textGameOverMessage.text = redTeamWins;
      } else {
        textGameOverMessage.text = tie;
      }
    }
	}
}

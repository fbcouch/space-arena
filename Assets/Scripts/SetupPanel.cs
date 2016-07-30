using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class SetupPanel : NetworkBehaviour {
  public Dropdown teamSize;
  public Dropdown gameLength;

  GameConfig gameConfig;

  public void TeamSizeChanged () {
    if (!isServer)
      return;
    (LobbyManager.singleton as LobbyManager).TeamSizeChanged (teamSize.value);
  }

  public void GameLengthChanged () {
    if (!isServer)
      return;
    (LobbyManager.singleton as LobbyManager).GameLengthChanged (gameLength.value);
  }

  public void FixedUpdate () {
    if (isServer)
      return;
    if (gameConfig == null) {
      GameObject gameObject = GameObject.FindGameObjectWithTag("GameConfig");
      if (gameObject != null)
        gameConfig = gameObject.GetComponent<GameConfig> ();
      return;
    }

    teamSize.interactable = false;
    gameLength.interactable = false;

    teamSize.value = gameConfig.teamSize;
    gameLength.value = gameConfig.gameLength;
  }
}

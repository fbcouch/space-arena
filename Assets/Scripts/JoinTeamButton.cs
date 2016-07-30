using UnityEngine;
using System.Collections;

public class JoinTeamButton : MonoBehaviour {
  public string team;

  LobbyManager lobbyManager;
  GameConfig gameConfig;

  void Start () {
    lobbyManager = LobbyManager.singleton as LobbyManager;
  }

  void Update () {
    GameObject gameObject = GameObject.FindGameObjectWithTag("GameConfig");
    if (gameObject != null)
      gameConfig = gameObject.GetComponent<GameConfig> ();
  }

  public void OnClick () {
    int i = 0;
    foreach (LobbyPlayer lobbyPlayer in lobbyManager.lobbySlots) {
      if (lobbyPlayer == null)
        continue;
      if (lobbyPlayer.team == team)
        i++;
    }

    if (i <= gameConfig.teamSize)
      LobbyPlayer.localPlayer.CmdSetTeam (team);
  }
}

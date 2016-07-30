using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameConfig : NetworkBehaviour {
  [SyncVar]
  public int gameLength = 4;
  [SyncVar]
  public int teamSize = 2;

  public static GameConfig instance;

  void Start () {
    DontDestroyOnLoad (gameObject);
    instance = this;
  }
}

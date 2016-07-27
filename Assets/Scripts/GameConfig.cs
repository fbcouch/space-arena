using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameConfig : NetworkBehaviour {
  [SyncVar]
  public int gameLength = 4;
  [SyncVar]
  public int teamSize = 2;
}

﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public struct GameHost {
  public int id;
  public string name;
  public string ip;
  public string port;
  public int cur_players;
  public int max_players;
  public string version;
  public string token;
}

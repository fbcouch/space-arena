using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LevelLoader : NetworkBehaviour {

	[ClientRpc]
	public void RpcLoadArena() {
		Application.LoadLevel ("arena");
	}
}

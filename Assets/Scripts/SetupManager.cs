using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Mono.Nat;

public class SetupManager : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject multiplayerSetupMenu;
  public GameObject multiplayerLobby;

	public GameObject playerNameInput;
	public GameObject musicToggle;
	public GameObject musicPlayer;
  public Dropdown shipSelector;

	public LobbyManager networkManager;

  public InputField serverAddressInput;
  public Toggle publicServer;

  public string apiUrl = "https://space-arena-api.herokuapp.com/game_hosts";
  public GameHostCollection gameHosts;
  public GameObject serverRowPrefab;
  public GameObject serverList;

  public static SetupManager instance;

	void Start () {
    instance = this;
    networkManager = GameObject.FindGameObjectWithTag ("NetworkManager").GetComponent<LobbyManager> ();
    if (GameConfig.instance != null)
      Destroy (GameConfig.instance.gameObject);
		RunOptions ();
    ShowMenu (mainMenu);

    NatUtility.DeviceFound += DeviceFound;
    NatUtility.DeviceLost += DeviceLost;
    NatUtility.StartDiscovery ();
	}

  private void DeviceFound (object sender, DeviceEventArgs args) {
    INatDevice device = args.Device;
    device.CreatePortMap (new Mapping (Protocol.Udp, 7777, 7777));
    Debug.Log ("External IP: " + device.GetExternalIP ().ToString ());
  }

  private void DeviceLost (object sender, DeviceEventArgs args) {
    INatDevice device = args.Device;
    device.DeletePortMap (new Mapping (Protocol.Udp, 7777, 7777));
  }

	public void OnExitClicked () {
		Debug.Log ("Exit");
		Application.Quit ();
	}

	public void OnMainMenuClicked () {
		Debug.Log ("To Main Menu");
		ShowMenu (mainMenu);
	}

	public void OnMultiplayerClicked () {
		Debug.Log ("To Multiplayer Menu");
		ShowMenu (multiplayerSetupMenu);

    StartCoroutine (FetchServers ());
	}

  IEnumerator FetchServers () {
    WWW w = new WWW (apiUrl + "?version=" + Version.version);

    yield return w;

    if (!string.IsNullOrEmpty (w.error)) {
      Debug.LogError (w.error);
    } else {
      Debug.Log (w.text);
      gameHosts = JsonUtility.FromJson<GameHostCollection> (w.text);
      GameObject prev = null;
      Debug.Log (gameHosts.data);
      RectTransform rectTransform = serverList.GetComponent<RectTransform> ();
      rectTransform.sizeDelta = new Vector2 (rectTransform.sizeDelta.x, gameHosts.data.Length * 37 + 20);
      foreach (GameHost gameHost in gameHosts.data) {
        GameObject serverRow = Instantiate (serverRowPrefab) as GameObject;
        serverRow.transform.SetParent (serverList.transform);
        serverRow.GetComponent<ServerRow> ().gameHost = gameHost;
      }
    }
  }

	public void OnOptionsMenuClicked () {
		Debug.Log ("To Options");
		LoadOptions ();
		ShowMenu (optionsMenu);
	}

	public void OnQuickStartClicked () {
		Debug.Log ("Quick Start");
    networkManager.isPublicServer = false;
		mainMenu.GetComponent<Canvas> ().enabled = false;
		networkManager.StartHost ();
		StartCoroutine (AutoReady ());
	}

	IEnumerator AutoReady () {
		yield return new WaitForSeconds (1);
    networkManager.lobbySlots [0].SendReadyToBeginMessage ();
	}

	public void OnHostGameClicked () {
    networkManager.isPublicServer = publicServer.isOn;
		networkManager.StartHost ();
	}

  public void StopHost () {
    networkManager.StopHost ();
  }

  public void OnJoinGameClicked () {
    networkManager.networkAddress = serverAddressInput.text;
    networkManager.StartClient ();
  }

  public void OnLobbyClientEnter () {
    ShowMenu (multiplayerLobby);
  }

  public void StopClient () {
    networkManager.StopClient ();
    SceneManager.LoadScene ("setup");
  }

	public void LoadOptions () {
		Debug.Log ("Load Options");
		if (PlayerPrefs.HasKey ("playerName")) {
			string playerName = PlayerPrefs.GetString ("playerName");
			playerNameInput.GetComponent<InputField> ().text = playerName;
		}

		if (PlayerPrefs.HasKey ("musicEnabled")) {
			bool musicEnabled = PlayerPrefs.GetInt ("musicEnabled") == 1;
			musicToggle.GetComponent<Toggle> ().isOn = musicEnabled;
		}

    shipSelector.ClearOptions ();
    var ships = new System.Collections.Generic.List<Dropdown.OptionData> ();
    foreach (ShipData shipData in ShipDataHolder.instance.shipData) {
      ships.Add (new Dropdown.OptionData (shipData.displayName));
    }
    shipSelector.AddOptions (ships);


    if (PlayerPrefs.HasKey ("shipIdentifier")) {
      for (int i = 0; i < ShipDataHolder.instance.shipData.Length; i++) {
        string shipIdentifier = PlayerPrefs.GetString ("shipIdentifier");
        if (ShipDataHolder.instance.shipData [i].identifier == shipIdentifier)
          shipSelector.value = i;
      }
    }
	}

	public void SaveOptions () {
		Debug.Log ("Save Options");
		PlayerPrefs.SetString ("playerName", playerNameInput.GetComponent<InputField> ().text);
		PlayerPrefs.SetInt ("musicEnabled", musicToggle.GetComponent<Toggle> ().isOn ? 1 : 0);
    PlayerPrefs.SetString ("shipIdentifier", ShipDataHolder.instance.shipData [shipSelector.value].identifier);
		PlayerPrefs.Save ();

		RunOptions ();
	}

	public void RunOptions () {
		if (PlayerPrefs.HasKey ("musicEnabled")) {
			bool musicEnabled = PlayerPrefs.GetInt ("musicEnabled") == 1;
			musicPlayer.GetComponent<AudioSource> ().enabled = musicEnabled;
		}
	}

	void ShowMenu (GameObject menu) {
		if (mainMenu != null)
			mainMenu.GetComponent<Canvas> ().enabled = false;
		if (optionsMenu != null)
			optionsMenu.GetComponent<Canvas> ().enabled = false;
		if (multiplayerSetupMenu != null)
			multiplayerSetupMenu.GetComponent<Canvas> ().enabled = false;
    if (multiplayerLobby != null)
      multiplayerLobby.GetComponent<Canvas> ().enabled = false;
		menu.GetComponent<Canvas> ().enabled = true;
	}
}

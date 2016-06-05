using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SetupManager : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject multiplayerSetupMenu;

	public GameObject playerNameInput;
	public GameObject musicToggle;
	public GameObject musicPlayer;

	public LobbyManager networkManager;

  public InputField serverAddressInput;

	void Start () {
		RunOptions ();
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
	}

	public void OnOptionsMenuClicked () {
		Debug.Log ("To Options");
		LoadOptions ();
		ShowMenu (optionsMenu);
	}

	public void OnQuickStartClicked () {
		Debug.Log ("Quick Start");
		mainMenu.GetComponent<Canvas> ().enabled = false;
		networkManager.StartHost ();
		StartCoroutine (AutoReady ());
	}

	IEnumerator AutoReady () {
		yield return new WaitForSeconds (1);
    networkManager.lobbySlots [0].SendReadyToBeginMessage ();
	}

	public void OnHostGameClicked () {
		networkManager.StartHost ();
	}

  public void StopHost () {
    networkManager.StopHost ();
  }

  public void OnJoinGameClicked () {
    networkManager.networkAddress = serverAddressInput.text;
    networkManager.StartClient ();
  }

  public void StopClient () {
    networkManager.StopClient ();
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
	}

	public void SaveOptions () {
		Debug.Log ("Save Options");
		PlayerPrefs.SetString ("playerName", playerNameInput.GetComponent<InputField> ().text);
		PlayerPrefs.SetInt ("musicEnabled", musicToggle.GetComponent<Toggle> ().isOn ? 1 : 0);
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
		menu.GetComponent<Canvas> ().enabled = true;
	}
}

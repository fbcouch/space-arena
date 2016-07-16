using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscMenu : MonoBehaviour {
  bool escPressed;
  Canvas canvas;

	// Use this for initialization
	void Start () {
    canvas = GetComponent <Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.GetButton ("Cancel") && !escPressed) {
      escPressed = true;
      canvas.enabled = !canvas.enabled;
    }
    escPressed = Input.GetButton ("Cancel");
	}

  public void OnResumePressed () {
    canvas.enabled = false;
  }

  public void OnExitPressed () {
    Debug.Log ("Exit Pressed");
    SceneManager.LoadScene (0);
  }
}

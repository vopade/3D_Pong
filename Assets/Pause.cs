using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
//----------------------------------------------------------------------------------------------------------------------#AWAKE
	void Awake() {
		Need.isPaused = true;
	}

//----------------------------------------------------------------------------------------------------------------------#PLAYGAME
	public void PlayGame() {
		// Load the previous scene
		Debug.Log("GAME: Go back to previous scene...");
		Need.newRound = true;
		Need.isPaused = !Need.isPaused;
		SceneManager.LoadScene (Need.last_scene);
		Need.last_scene = Need.scene_index;
	}

//----------------------------------------------------------------------------------------------------------------------#LOADMAINMENU
	public void LoadMainMenu() {
		// Load the Menu scene
		Debug.Log("GAME: Go back to Menu scene...");
		Need.isPaused = !Need.isPaused;
		SceneManager.LoadScene(0);
		Need.last_scene = Need.scene_index;
	}

//----------------------------------------------------------------------------------------------------------------------#FUNCTIONS
	public void SwitchFPS(Toggle check) {
		Debug.Log ("GAME: showFPS is " + check.isOn);
		Need.showFPS = check.isOn;
	}
}

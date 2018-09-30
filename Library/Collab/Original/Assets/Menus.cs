//Drag and drop the Menu Object from the hierarchy over the on ... event field
// TODO: Bei flaschen Namen anegeben, dass Name angegene werden muss
// TODO: FPS anzeigen lassen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
	private bool showFPS;
    private bool isPaused;
    private string playerName1 = "Player_1";
    private string playerName2 = "Player_2";

    public void PlayGame() {
        //If Player_? is unchanged; chekcen, dass spielernamen geändert
        if (playerName1 != "Player_1" || playerName1 != "Player_1" && playerName2 != "Player_2") {
            Debug.Log("GAME: Load next scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

	public void QuitGame() {
		Debug.Log("GAME: Quit...");
		Application.Quit();
	}

	public void SetSingleplayer() {
		//Debug.Log("MENU: Set player2 to computer...");
		Player.singlePlayer = true;
    }

	public void SetMultiplayer() {
		//Debug.Log("MENU: Set player2 to human...");
		Player.singlePlayer = false;
    }

	public void SetSpeed(Slider slider) {
        //Debug.Log("MENU: speed is set to: " + slider.value);
        Ball.speed = slider.value;
    }

    public void SetPlayer1Name(InputField field) {
        //Debug.Log("MENU: playerName1 is set to: " + field.text);
        playerName1 = field.text;
    }

    public void SetPlayer2Name(InputField field) {
        //Debug.Log("MENU: playerName2 is set to: " + field.text);
        playerName2 = field.text;
    }

    public void SwitchFPS() {
	    
	}

	public void Resume() {
		Time.timeScale = 1;
	}

	public void ExitMatch() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}
}
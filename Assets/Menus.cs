using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#PLAYGAME
    public void PlayGame()
    {
        //If Player_? is unchanged; chekcen, dass spielernamen geändert
		if (((Need.player1_name != "Player_1") || (Need.player1_name != "Player_1" && Need.player2_name != "Player_2")) && Need.player1_name != Need.player2_name)
        {
            Debug.Log("GAME: Load next scene...");
			Need.player1_lifes = 3;
			Need.player2_lifes = 3;
			Need.newRound = true;
			Need.isGame = true;
			SceneManager.LoadScene (1);//SceneManager.GetActiveScene().buildIndex + 1);
			Need.last_scene = Need.scene_index;
        }
    }

//----------------------------------------------------------------------------------------------------------------------#QUITGAME
	public void QuitGame() {
		Debug.Log("GAME: Quit...");
		Application.Quit();
	}

//----------------------------------------------------------------------------------------------------------------------#FUNCTIONS
	public void SetSingleplayer() {
		//Debug.Log("MENU: Set player2 to computer...");
		Need.isSinglePlayer = true;
    }

	public void SetMultiplayer() {
		//Debug.Log("MENU: Set player2 to human...");
		Need.isSinglePlayer = false;
    }

	public void SetSpeed(Slider slider) {
        Debug.Log("MENU: speed is set to: " + slider.value);
		// range: 0.5 - 3.0 
		if (slider.value > 0.5f) {
			Need.ball_speed = slider.value;
		}
    }

    public void SetPlayer1Name(InputField field)
    {
        //Debug.Log("MENU: playerName1 is set to: " + field.text);
		Need.player1_name = field.text;
    }

    public void SetPlayer2Name(InputField field)
    {
        //Debug.Log("MENU: playerName2 is set to: " + field.text);
		Need.player2_name = field.text;
    }

    public void SwitchFPS(Toggle check) {
		//Debug.Log ("GAME: showFPS is " + check.isOn);
		Need.showFPS = check.isOn;
	}
}
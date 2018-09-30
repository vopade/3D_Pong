using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playarea : MonoBehaviour {

	private Ball ball;
	private Ball_Retro ballRetro;

	void OnCollisionEnter(Collision col) {
		if (Need.scene_index == 1) {
			ball = col.gameObject.GetComponent<Ball> ();
		} else {
			ballRetro = col.gameObject.GetComponent<Ball_Retro> ();
		}
		if(ball != null || ballRetro != null) {
			//Who am I and what I'm doing -> cases for every side
			switch(gameObject.name) {
			case "Playarea_Bottom":
                Audio.audioFlags[(int)Audio.AudioFiles.BallBounce] = true;
				Debug.Log("Playarea_Bottom touched");
				break;
			case "Playarea_Top":
                Audio.audioFlags[(int)Audio.AudioFiles.BallBounce] = true;
				Debug.Log("Playarea_Top touched");
				break;
			case "Playarea_Back":
                Audio.audioFlags[(int)Audio.AudioFiles.BallBounce] = true;
				Debug.Log("Playarea_Back touched");
				break;
			case "Playarea_Front":
                Audio.audioFlags[(int)Audio.AudioFiles.BallBounce] = true;
				Debug.Log("Playarea_Front touched");
				break;
			case "Playarea_Left":
                Audio.audioFlags[(int)Audio.AudioFiles.GameEnd] = true;
				Need.player1_points++;
                Ball.firstSound = true;
                ball.startNewGame();
				Debug.Log("Player_2 hat gepunktet");
				break;
			case "Playarea_Right":
                Audio.audioFlags[(int)Audio.AudioFiles.GameEnd] = true;
				Need.player2_points++;
                Ball.firstSound = true;
                ball.startNewGame();
				Debug.Log("Player_1 hat gepunktet");
				break;
			}
			Debug.Log("Neuer Zwischenstand: " + Need.player1_points + ":" + Need.player2_points);
			//Set the Ball to the center, but don't change the initial vector!!!
			if (ball != null) {
				ball.transform.position = new Vector3 (0f, 0f, 0f);
			} else {
				ballRetro.transform.position = new Vector3 (0f, 0f, 0f);
			}
		}
	}
}
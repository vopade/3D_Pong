using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playarea : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the playarea
	private Ball ball;				// Das Skript auf dem Ball
	private Ball_Retro ballRetro;	// Das Skript auf dem retro Ball

//----------------------------------------------------------------------------------------------------------------------#ONCOLLISIONENTER
	void OnCollisionEnter(Collision col) {
		//Debug.Log ("PLAYAREA: Name of the gameobject: "+gameObject.name);
		if (Need.scene_index == 1) {
			ball = col.gameObject.GetComponent<Ball> ();
		} else {
			ballRetro = col.gameObject.GetComponent<Ball_Retro> ();
		}
		if(ball != null || ballRetro != null) {
			//Who am I and what I'm doing -> cases for every side
			switch(gameObject.name) {
				//Nomination here with DARK and LIGHT!!!
				case "Playarea_Bottom": case "Playarea_Bottom_Grid_Dark": case "Playarea_Bottom_Grid_Light":
	                Audio.audioFlags[(int)Audio.AudioFiles.BallBounce] = true;
					//Debug.Log("Playarea_Bottom touched");
					break;
				case "Playarea_Top": case "Playarea_Top_Grid_Dark": case "Playarea_Top_Grid_Light":
	                Audio.audioFlags[(int)Audio.AudioFiles.BallBounce] = true;
					//Debug.Log("Playarea_Top touched");
					break;
				case "Playarea_Back": case "Playarea_Back_Grid_Dark": case "Playarea_Back_Grid_Light":
	                Audio.audioFlags[(int)Audio.AudioFiles.BallBounce] = true;
					//Debug.Log("Playarea_Back touched");
					break;
				case "Playarea_Front": case "Playarea_Front_Grid_Dark": case "Playarea_Front_Grid_Light":
	                Audio.audioFlags[(int)Audio.AudioFiles.BallBounce] = true;
					//Debug.Log("Playarea_Front touched");
					break;
				//Two additional cases for the new grids, beacuse they are in front of the walls
				case "Playarea_Left": case "Playarea_Left_Grid (1)": case "Playarea_Left_Grid (2)":
					Debug.Log ("Player_1 hat gepunktet");
					Need.lastPoint = Need.player1_name;
					Need.isPoint = true;
					Audio.audioFlags [(int)Audio.AudioFiles.GameEnd] = true;
					if (Need.scene_index == 1) {
						Ball.firstSound = true;
					}
					break;
				case "Playarea_Right": case "Playarea_Right_Grid (1)": case "Playarea_Right_Grid (2)":
					Debug.Log ("Player_2 hat gepunktet");
					Need.lastPoint = Need.player2_name;
					Need.isPoint = true;
					Audio.audioFlags [(int)Audio.AudioFiles.GameEnd] = true;
					if (Need.scene_index == 1) {
						Ball.firstSound = true;
					}
					break;
			}
		}
	}
}

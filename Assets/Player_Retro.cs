using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Retro : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the retroplayer
	//private bool up;					// Wenn nach oben korrigiert werden muss
	//private bool down;				// Wenn nach unten korrigiert werden muss
	//private float oldPos;				// Die alte Position
	//private float newPos;				// Die neue Position
	private Vector3 forwardP1;			// Der Wert, um den sich Spieler 1 bewegen kann
	private Vector3 forwardP2;			// Der Wert, um den sich Spieler 2 bewegen kann
	private float defaultVelocity;		// Die Standardgeschwindigkeit der Spieler

//----------------------------------------------------------------------------------------------------------------------#START
	void Start()
	{
		// Init the class variables
		forwardP1 = new Vector3(0f, 2f, 0f);
		forwardP2 = new Vector3(0f, 2f, 0f);
        defaultVelocity = Need.defaultVelocityPanel * 0.6f;
	}

//----------------------------------------------------------------------------------------------------------------------#FIXEDUPDATE
	void FixedUpdate()
	{
		// Steuerung unter Berücksichtugung von Geschwindigkeit und Invertierung
		if (gameObject.name == "Player_1") {
			if (Input.GetButton("Y_Player_1")) {
				transform.localPosition += Input.GetAxisRaw("Y_Player_1") * forwardP1 * defaultVelocity * Time.deltaTime;
			}
		} else {
			if (!Need.isSinglePlayer) {
				//Attention -> - because the view of the secons player is inverse...
				if (Input.GetButton ("Y_Player_2")) {
					transform.localPosition += Input.GetAxisRaw ("Y_Player_2") * forwardP2 * defaultVelocity * Time.deltaTime;
				}
			} else {
//				if (Need.ball_direction == 1) {
//					//+/-10 is some buffer of the player hight
//					if (transform.localPosition.y > Need.ball_position_y +5f && !down) {
//						down = true;
//						oldPos = transform.localPosition.y;
//						newPos = transform.localPosition.y -12.5f;
//					}
//					if (down) {
//						transform.localPosition -= forwardP1 * defaultVelocity * Time.deltaTime;
//						if (transform.localPosition.y <= newPos || transform.localPosition.y == -37.5f) {
//							down = false;
//						}
//					}
//					if (transform.localPosition.y < Need.ball_position_y -5f && !up) {
//						up = true;
//						oldPos = transform.localPosition.y;
//						newPos = transform.localPosition.y +12.5f;
//					}
//					if (up) {
//						transform.localPosition += forwardP1 * defaultVelocity * Time.deltaTime;
//						if (transform.localPosition.y >= newPos || transform.localPosition.y == +37.5f) {
//							up = false;
//						}
//					}
//				}
				//Direct following could be not smooth -> you can't win :P
				if (Need.ball_position_y > -37.5f && Need.ball_position_y < 37.5f) {
					transform.localPosition = new Vector3 (-85f, Need.ball_position_y, 0f);
				}
			}
		}
	}
}
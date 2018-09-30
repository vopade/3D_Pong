/*
 * Use this for initialization
 * Hinter Spieler 2 ist -100, 0, 0
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rearViewCamera : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the camera
	private bool cameraIsShaking;							// Sagt ob die Kamera wackelt
	private Vector3 shakingVector;							// Gibt den Wackelvektor an ;)
	private byte cameraShakingState;						// Gibt status über den Fortschritt des Wackelns
	private float timerCameraShaking;						// Gibt die Zeit des Wackelns an

	private GameObject canvas;								// Das UI auf dem alles liegt

	// Def & init constants of class camera
	private const float cameraShakingBoundary = 0.3f;		// Grenzwert des Wackelns
	int frequencyCnt = 0;
	private const int frequency = 20;

//----------------------------------------------------------------------------------------------------------------------#START
	void Start() {
		// Init the class variables
		cameraShakingState = 0;
		cameraIsShaking = false;
		canvas = GameObject.Find ("Canvas").gameObject;
		timerCameraShaking = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationCameraShaking];
		//GameObject.Find ("Playarea_Left").SetActive (true); // schwarz
		if (Need.goesToPlayer1) {
			deactiveWallBehindPlayer1 ();
			changeUIColorToPlayer2 ();
		} else {
			deactiveWallBehindPlayer2 ();
			changeUIColorToPlayer1 ();
		}
	}

//----------------------------------------------------------------------------------------------------------------------#UPDATE
	void Update ()
    {
		//Debug.Log ("CAMERA: Balldirection: "+Need.ball_direction);

		// PLEASE RUN ONLY ONCE!!! INPERFORMANT LIKE SH**
		if (Need.isSinglePlayer) {
			setCameraBehindPlayer1 ();
			deactiveWallBehindPlayer1 ();
			changeUIColorToPlayer2 ();
		} else {
			if (Need.ball_direction == 0) {
				setCameraBehindPlayer1 ();
				deactiveWallBehindPlayer1 ();
				changeUIColorToPlayer2 ();
			}

			if (Need.ball_direction == 1) {
				setCameraBehindPlayer2 ();
				deactiveWallBehindPlayer2 ();
				changeUIColorToPlayer1 ();
			}
		}

		if (ItemHandler.eventFlags [1]) {
			if (!cameraIsShaking) {
				Debug.Log ("EVENT: Shaking beginnt. Kameraposition: " + transform.position);
				ItemHandler.userReadableNameCurrentActiveEvent = "EARTHQUAKE! EVERYONE TAKE SHELTER";

				cameraIsShaking = true;
			}
			if (cameraShakingState == 0) {
				shakingVector = new Vector3 (0,
					Random.Range (-cameraShakingBoundary, cameraShakingBoundary),
					Random.Range (-cameraShakingBoundary, cameraShakingBoundary));
				transform.position += shakingVector;
				//transform.position += new Vector3(5,5,5);
				frequencyCnt++;
				if (frequencyCnt > frequency) 
				{
					cameraShakingState = 1;
					frequencyCnt = 0;
				}
			}

			if (cameraShakingState == 1) 
			{
				transform.position -= shakingVector;
				cameraShakingState = 0;
				frequencyCnt++;
				if (frequencyCnt > frequency) 
				{
					cameraShakingState = 1;
					frequencyCnt = 0;
				}
			}

			timerCameraShaking -= Time.deltaTime;
			if (timerCameraShaking < 0.0f) {
				Need.isItem = false;
				ItemHandler.eventFlags [1] = false;
				timerCameraShaking = ItemHandler.eventSpecifications [(int)ItemHandler.EventSpecifications.durationCameraShaking];
				ItemHandler.userReadableNameCurrentActiveEvent = "";
				Debug.Log ("EVENT: Beende Shaking. Kameraposition: " + transform.position);
			}

		}
    }

	//----------------------------------------------------------------------------------------------------------------------#FUNCTIONS

	private void deactiveWallBehindPlayer1()
	{
		foreach (GameObject ele in GameObject.FindGameObjectsWithTag ("Playarea_Left")) // schwarz, p2
		{
			ele.GetComponent<MeshRenderer>().enabled = true;
		}

		foreach (GameObject ele in GameObject.FindGameObjectsWithTag ("Playarea_Right")) 
		{
			ele.GetComponent<MeshRenderer>().enabled = false;
		}
	}
	private void deactiveWallBehindPlayer2()
	{
		foreach (GameObject ele in GameObject.FindGameObjectsWithTag ("Playarea_Left")) // schwarz, p2
		{
			ele.GetComponent<MeshRenderer>().enabled = false;
		}

		foreach (GameObject ele in GameObject.FindGameObjectsWithTag ("Playarea_Right")) 
		{
			ele.GetComponent<MeshRenderer>().enabled = true;
		}
	}

    private void setCameraBehindPlayer1() {
        //this.transform.position = new Vector3(150.0f, 0.0f, 0.0f);
        //transform.eulerAngles = new Vector3(0.0f, -90, 0.0f);
		this.transform.position = new Vector3(12.0f, 0.0f, 0.0f);
		transform.eulerAngles = new Vector3(0.0f, -90, 0.0f);

    }

    private void setCameraBehindPlayer2() {
        //this.transform.position = new Vector3(-150.0f, 0.0f, 0.0f);
        //transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
		this.transform.position = new Vector3(-12.0f, 0.0f, 0.0f);
		transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
    }

	private void changeUIColorToPlayer1 () {
		canvas.transform.GetChild (0).GetComponent<Text> ().color = Color.white;
		canvas.transform.GetChild (1).GetComponent<Text> ().color = Color.white;
		canvas.transform.GetChild (2).GetComponent<Text> ().color = Color.black;
		canvas.transform.GetChild (7).GetComponent<Text> ().color = Color.white;
		canvas.transform.GetChild (8).GetComponent<Text> ().color = Color.white;
	}

	private void changeUIColorToPlayer2 () {
		canvas.transform.GetChild (0).GetComponent<Text> ().color = Color.black;
		canvas.transform.GetChild (1).GetComponent<Text> ().color = Color.black;
		canvas.transform.GetChild (2).GetComponent<Text> ().color = Color.white;
		canvas.transform.GetChild (7).GetComponent<Text> ().color = Color.black;
		canvas.transform.GetChild (8).GetComponent<Text> ().color = Color.black;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIndicator : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the ballindicator
	private float sizeX;							// Die x_Größe des Indikators
	private float sizeY;							// Die y_Größe des Indikators
	private float sizeZ;							// Die z_Größe des Indikators
	private float inversSizeX;						// Die inverse x_Größe des Indikators
	private float inversSizeY;						// Die inverse y_Größe des Indikators
	private float inversSizeZ;						// Die inverse z_Größe des Indikators

	private GameObject top;							// Der Indikator von oben
	private GameObject back;						// Der Indikator von hinten
	private GameObject left;						// Der Indikator von links
	private GameObject right;						// Der Indikator von rechts
	private GameObject front;						// Der Indikator von vorne
	private GameObject bottom;						// Der Indikator vom boden
	private float indicatorPosX;					// Die Position des Indikators
	private float indicatorPosY;					// Die Position des Indikators
	private float indicatorPosZ;					// Die Position des Indikators

	// Def class variables for the ballindicator
	public Material red;							// Die Farbe rot als Material
	public Material green;							// Die Farbe grün als Material
	public Material yellow;							// Die Farbe gelb als Material

	// Def & init constants of class ballindicator
	private const byte playareaSizeX = 50;			// Die Größe des Spielfeldes
	private const byte playareaSizeY = 50;			// Die Größe des Spielfeldes
	private const float playareaSizeZ = 88.889f;	// Die Größe des Spielfeldes
	//private const float scaleFactor = 2.5f;		// Ein Faktor zur besseren Skalierung

//----------------------------------------------------------------------------------------------------------------------#START
	void Start() {
		// Init class variables
		sizeX = 1;
		sizeY = 1;
		sizeZ = 1;
		indicatorPosX = playareaSizeX - 0.5f;
		indicatorPosY = playareaSizeY - 0.5f;
		indicatorPosZ = playareaSizeZ - 0.5f;
		top = GameObject.Find ("Ball_Top");
		back = GameObject.Find ("Ball_Back");
		left = GameObject.Find ("Ball_Left");
		right = GameObject.Find ("Ball_Right");
		front = GameObject.Find ("Ball_Front");
		bottom = GameObject.Find ("Ball_Bottom");
	}

//----------------------------------------------------------------------------------------------------------------------#ONCOLLISIONENTER
	void OnCollisionEnter(Collision col) {
		switch(gameObject.name) {
			case "Ball_Right":
				right.GetComponent<MeshRenderer> ().material = red;
				break;
			case "Ball_Left":
				left.GetComponent<MeshRenderer> ().material = red;
				break;
			case "Ball_Top":
				top.GetComponent<MeshRenderer> ().material = red;
				break;
			case "Ball_Bottom":
				bottom.GetComponent<MeshRenderer> ().material = red;
				break;
			case "Ball_Back":
				back.GetComponent<MeshRenderer> ().material = red;
				break;
			case "Ball_Front":
				front.GetComponent<MeshRenderer> ().material = red;
				break;
		}
	}

//----------------------------------------------------------------------------------------------------------------------#FIXEDUPDATE
	void FixedUpdate() {
		sizeX = (Need.ball_position_x + playareaSizeX) / playareaSizeX;
		// The invers value have to be negated
		inversSizeX = (-Need.ball_position_x + playareaSizeX) / playareaSizeX;
		// Scale the indicator proportional to the Field use the scaleFactor to calibrate the size
		right.transform.localScale = new Vector3(0.1f, sizeX, sizeX);// * scaleFactor, sizeX * scaleFactor);
		// Scale the invers indicator with the invers size
		left.transform.localScale = new Vector3(0.1f, inversSizeX, inversSizeX);
		if (Need.ball_direction_x) {
			right.GetComponent<MeshRenderer> ().material = yellow;
			left.GetComponent<MeshRenderer> ().material = green;
		} else {
			left.GetComponent<MeshRenderer> ().material = yellow;
			right.GetComponent<MeshRenderer> ().material = green;
		}


		sizeY = (Need.ball_position_y + playareaSizeY) / playareaSizeY;
		inversSizeY = (-Need.ball_position_y + playareaSizeY) / playareaSizeY;
		top.transform.localScale = new Vector3(sizeY, 0.1f, sizeY);
		bottom.transform.localScale = new Vector3(inversSizeY, 0.1f, inversSizeY);
		if (Need.ball_direction_y) {
			top.GetComponent<MeshRenderer> ().material = yellow;
			bottom.GetComponent<MeshRenderer> ().material = green;
		} else {
			bottom.GetComponent<MeshRenderer> ().material = yellow;
			top.GetComponent<MeshRenderer> ().material = green;
		}

		sizeZ = (Need.ball_position_z + playareaSizeZ) / playareaSizeZ;
		inversSizeZ = (-Need.ball_position_z + playareaSizeZ) / playareaSizeZ;
		back.transform.localScale = new Vector3(sizeZ, sizeZ, 0.1f);
		front.transform.localScale = new Vector3(inversSizeZ, inversSizeZ, 0.1f);
		if (Need.ball_direction_z) {
			back.GetComponent<MeshRenderer> ().material = yellow;
			front.GetComponent<MeshRenderer> ().material = green;
		} else {
			front.GetComponent<MeshRenderer> ().material = yellow;
			back.GetComponent<MeshRenderer> ().material = green;
		}

		// Transform the indicators to the right positions
		right.transform.localPosition = new Vector3(indicatorPosX, Need.ball_position_y, Need.ball_position_z);
		left.transform.localPosition = new Vector3(-indicatorPosX, Need.ball_position_y, Need.ball_position_z);

		top.transform.localPosition = new Vector3(Need.ball_position_x, indicatorPosY, Need.ball_position_z);
		bottom.transform.localPosition = new Vector3(Need.ball_position_x, -indicatorPosY, Need.ball_position_z);

		back.transform.localPosition = new Vector3(Need.ball_position_x, Need.ball_position_y, indicatorPosZ);
		front.transform.localPosition = new Vector3(Need.ball_position_x, Need.ball_position_y, -indicatorPosZ);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
	//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the ballindicator
	private byte temp_x;						// Die Position des Felds für x (alter Wert)
	private byte temp_y;						// Die Position des Felds für y (alter Wert)
	private byte temp_z;						// Die Position des Felds für z (alter Wert)
	private byte brik_x;						// Die Position des Felds für x
	private byte brik_y;						// Die Position des Felds für y
	private byte brik_z;						// Die Position des Felds für z
	private GameObject right;					// Die GameObjects der Playarea
	private GameObject left;					// Die GameObjects der Playarea
	private GameObject top;						// Die GameObjects der Playarea
	private GameObject bottom;					// Die GameObjects der Playarea
	private GameObject back;					// Die GameObjects der Playarea
	private GameObject front;					// Die GameObjects der Playarea


	// Def class variables for the ballindicator
	public Material default_dark;				// Die Farbe schwarz als Material
	public Material default_light;				// Die Farbe weiß als Material
	public Material default_normal;				// Die Farbe xyz als Material
	public Material hover_dark;					// Die Farbe schwarz_heller als Material
	public Material hover_light;				// Die Farbe weiß_heller als Material
	public Material hover_normal;				// Die Farbe xyz_heller als Material
	public Material touch_dark;					// Die Farbe silber bei Berührung
	public Material touch_light;				// Die Farbe gold bei Berührung
	public Material touch_normal;				// Die Farbe xyz als Material

	// Def & init constants of class ballindicator
	private const byte playareaSizeX = 5;		// Die Größe des Spielfeldes
	private const byte playareaSizeY = 3;		// Die Größe des Spielfeldes
	private const byte playareaSizeZ = 5;		// Die Größe des Spielfeldes
	//private const float scaleFactor = 2.5f;	// Ein Faktor zur besseren Skalierung

	//----------------------------------------------------------------------------------------------------------------------#Start
	void Start() {
		// Init class variables
		temp_x = 0;
		temp_y = 0;
		temp_z = 0;
		right = GameObject.Find("Playarea_Right");
		left = GameObject.Find("Playarea_Left");
		top = GameObject.Find("Playarea_Top");
		bottom = GameObject.Find("Playarea_Bottom");
		back = GameObject.Find("Playarea_Back");
		front = GameObject.Find("Playarea_Front");
	}

	//----------------------------------------------------------------------------------------------------------------------#ONCOLLISIONENTER
	void OnCollisionEnter(Collision col) {
		//#Müsste schon ausreichen
		if (Need.ball_position_x < -0.5f) {
			this.GetComponent<MeshRenderer> ().material = touch_dark;
		} else if (Need.ball_position_x > 0.5f) {
			this.GetComponent<MeshRenderer> ().material = touch_light;
		} else {
			this.GetComponent<MeshRenderer> ().material = touch_normal;
		}
		// Incremental spread of coloring -> like some animation
	}

	//----------------------------------------------------------------------------------------------------------------------#FIXEDUPDATE
	void FixedUpdate() {
		brik_x = calcAffectedBriks (Need.ball_position_x, 'x');
		brik_y = calcAffectedBriks (Need.ball_position_y, 'y');
		brik_z = calcAffectedBriks (Need.ball_position_z, 'z');

		// Change the color back, if a new brik is in the focus
//		if (brik_x != temp_x || brik_y != temp_y || brik_z != temp_z) {
//			// Back-change the x sides
//			if (right) {
//				right.transform.GetChild (temp_y).GetChild (temp_z).GetComponent<MeshRenderer> ().material = default_light;
//			}
//			if (left) {
//				left.transform.GetChild (temp_y).GetChild (temp_z).GetComponent<MeshRenderer> ().material = default_dark;
//			}
//			// Back-change the y sides
//			if (top || bottom) {
//				if (temp_x < (playareaSizeX * 2)) {
//					top.transform.GetChild (temp_x).GetChild (temp_z).GetComponent<MeshRenderer> ().material = default_light;
//					bottom.transform.GetChild (temp_x).GetChild (temp_z).GetComponent<MeshRenderer> ().material = default_light;
//				}
//				if (temp_x == (playareaSizeX * 2)) {
//					top.transform.GetChild (temp_x).GetChild (temp_z).GetComponent<MeshRenderer> ().material = default_normal;
//					bottom.transform.GetChild (temp_x).GetChild (temp_z).GetComponent<MeshRenderer> ().material = default_normal;
//				}
//				if (temp_x > (playareaSizeX * 2)) {
//					top.transform.GetChild (temp_x).GetChild (temp_z).GetComponent<MeshRenderer> ().material = default_dark;
//					bottom.transform.GetChild (temp_x).GetChild (temp_z).GetComponent<MeshRenderer> ().material = default_dark;
//				}
//			}
//			if (back || front) {
//				// Same variable, because the side is transformed!!!
//				if (temp_x < (playareaSizeX * 2)) {
//					back.transform.GetChild (temp_y).GetChild (temp_x).GetComponent<MeshRenderer> ().material = default_light;
//				}
//				if (temp_x == (playareaSizeX * 2)) {
//					back.transform.GetChild (temp_y).GetChild (temp_x).GetComponent<MeshRenderer> ().material = default_normal;
//				}
//				if (temp_x > (playareaSizeX * 2)) {
//					back.transform.GetChild (temp_y).GetChild (temp_x).GetComponent<MeshRenderer> ().material = default_dark;
//				}
//			}
//			temp_x = brik_x;
//			temp_y = brik_y;
//			temp_z = brik_z;
//		}
		//Debug.Log ("INDICATOR: brik_x: "+brik_x);
		//Debug.Log ("INDICATOR: brik_y: "+brik_y);
		//Debug.Log ("INDICATOR: brik_z: "+brik_z);

		// If the ball comes to this side
		if (Need.ball_direction_x) {
			if (Need.ball_position_x >= 0.25f) {
				//gameObject.name == "Playarea_Right"
				if (transform.GetChild(0)) {
					//Da keine Wand momentan existiert!!!
					right.transform.GetChild (brik_y).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_light;
				}
			}
			//else {
			//	if (gameObject.name == "Playarea_Right") {
			//		this.transform.GetChild (brik_y).GetChild (brik_z-1).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_y-1).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_y).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_y+1).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_y).GetChild (brik_z+1).GetComponent<MeshRenderer> ().material = hover_light;
			//	}
			//}
		// Do the same for the opposing side
		} else {
			if (Need.ball_position_x <= -0.25f) {
				//gameObject.name == "Playarea_Left"
				if (transform.GetChild(2)) {
					left.transform.GetChild (brik_y).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_dark;
				}
			}
			//else {
			//	if (gameObject.name == "Playarea_Left") {
			//		this.transform.GetChild (brik_y).GetChild (brik_z-1).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_y-1).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_y).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_y+1).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_y).GetChild (brik_z+1).GetComponent<MeshRenderer> ().material = hover_dark;
			//	}
			//}
		}
			
		if (Need.ball_direction_y) {
			if (Need.ball_position_y >= 0.25f) {
				//gameObject.name == "Playarea_Top"
				if (transform.GetChild(4)) {
					top.transform.GetChild (brik_x).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_light;
				}
			}
			//else {
			//	if (gameObject.name == "Playarea_Top") {
			//		this.transform.GetChild (brik_x).GetChild (brik_z-1).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_x-1).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_x).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_x+1).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_x).GetChild (brik_z+1).GetComponent<MeshRenderer> ().material = hover_light;
			//	}
			//}
		} else {
			if (Need.ball_position_y <= -0.25f) {
				//gameObject.name == "Playarea_Bottom"
				if (transform.GetChild(6)) {
					bottom.transform.GetChild (brik_x).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_dark;
				}
			}
			//else {
			//	if (gameObject.name == "Playarea_Bottom") {
			//		this.transform.GetChild (brik_x).GetChild (brik_z-1).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_x-1).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_x).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_x+1).GetChild (brik_z).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_x).GetChild (brik_z+1).GetComponent<MeshRenderer> ().material = hover_dark;
			//	}
			//}
		}
			
		if (Need.ball_direction_z) {
			if (Need.ball_position_z >= 0.25f) {
				//gameObject.name == "Playarea_Back"
				if (transform.GetChild(8)) {
					back.transform.GetChild (brik_y).GetChild (brik_x).GetComponent<MeshRenderer> ().material = hover_light;
				}
			}
			//else {
			//	if (gameObject.name == "Playarea_Back") {
			//		this.transform.GetChild (brik_y).GetChild (brik_x-1).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_y-1).GetChild (brik_x).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_y).GetChild (brik_x).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_y+1).GetChild (brik_x).GetComponent<MeshRenderer> ().material = hover_light;
			//		this.transform.GetChild (brik_y).GetChild (brik_x+1).GetComponent<MeshRenderer> ().material = hover_light;
			//	}
			//}
		} else {
			if (Need.ball_position_z <= -0.25f) {
				//gameObject.name == "Playarea_Front"
				if (transform.GetChild(8)) {
					front.transform.GetChild (brik_y).GetChild (brik_x).GetComponent<MeshRenderer> ().material = hover_dark;
				}
			}
			//else {
			//	if (gameObject.name == "Playarea_Front") {
			//		this.transform.GetChild (brik_y).GetChild (brik_x-1).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_y-1).GetChild (brik_x).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_y).GetChild (brik_x).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_y+1).GetChild (brik_x).GetComponent<MeshRenderer> ().material = hover_dark;
			//		this.transform.GetChild (brik_y).GetChild (brik_x+1).GetComponent<MeshRenderer> ().material = hover_dark;
			//	}
			//}
		}

		// Normal could be implemented later as an exception
		// Exception for the case, that the panel is at the position of possible collision -> Need.isOnPannel;
	}

	private byte calcAffectedBriks(float pos, char axis) {
		byte j = 0;
		switch (axis) {
			case 'x':
				for (float i = playareaSizeX + 0.25f; i >= -playareaSizeX + 0.25f; i -= 0.5f) {
					if (pos < i && pos > (i - 0.5f)) {
						break;
					}
					j++;
				}
				break;
			case 'y':
				for (float i = playareaSizeY + 0.25f; i >= -playareaSizeY + 0.25f; i -= 0.5f) {
					if (pos < i && pos > (i - 0.5f)) {
						break;
					}
					j++;
				}
				break;
			case 'z':
				for (float i = playareaSizeZ + 0.25f; i >= -playareaSizeZ + 0.25f; i -= 0.5f) {
					if (pos < i && pos > (i - 0.5f)) {
						break;
					}
					j++;
				}
				break;
		}
		return j;
	}
}

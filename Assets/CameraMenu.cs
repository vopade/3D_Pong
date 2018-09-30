using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the camera
	private float xrot;
	private float yrot;

	// Def & init constants of class camera
	private const float max = 1.5f;
	private const float smooth = 10f;

//----------------------------------------------------------------------------------------------------------------------#START
	void Start()
	{
		// Init the class variables
		xrot = 0f;
		yrot = 0f;
	}

//----------------------------------------------------------------------------------------------------------------------#FIXEDUPDATE
	void FixedUpdate()
	{
		if (xrot > max) {
			if ((xrot + (Input.GetAxis ("Mouse X") / smooth)) < max) {
				xrot += (Input.GetAxis ("Mouse X") / smooth);
			}
		} else if (xrot < -max) {
			if ((xrot + (Input.GetAxis ("Mouse X") / smooth)) > -max) {
				xrot += (Input.GetAxis ("Mouse X") / smooth);
			}
		} else {
			if (xrot >= -max && xrot <= max) {
				xrot += (Input.GetAxis ("Mouse X") / smooth);
			}
		}
		if (yrot > max) {
			if ((yrot + (Input.GetAxis ("Mouse Y") / smooth)) < max) {
				yrot += (Input.GetAxis ("Mouse Y") / smooth);
			}
		} else if (yrot < -max) {
			if ((yrot + (Input.GetAxis ("Mouse Y") / smooth)) > -max) {
				yrot += (Input.GetAxis ("Mouse Y") / smooth);
			}
		} else {
			if (yrot >= -max && yrot <= max) {
				yrot += (Input.GetAxis ("Mouse Y") / smooth);
			}
		}
			transform.eulerAngles = new Vector3 (yrot, xrot, 0.0f);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the camera
	private bool cameraIsShaking;							// Sagt ob die Kamera wackelt
	private Vector3 shakingVector;							// Gibt den Wackelvektor an ;)
	private Vector3 wantedPosition;							// Gibt die erwünschte Position an
	private byte cameraShakingState;						// Gibt status über den Fortschritt des Wackelns
	private float timerCameraShaking;						// Gibt die Zeit des Wackelns an

	// Def class variables for the camera
    public Transform target;

	// Def & init constants of class camera
	private const float height = 3.0f;						// Die Höhe der Kamera
	private const float damping = 5.0f;						// ???
	private const float distance = 3.0f;					// Die Entfernung zum Objekt
	private const float transformX = 100f;					// Die X Position der Kamera
	private const float transformY = 35.0f;					// Die Y Position der Kamera
	private const float transformZ = 40.0f;					// Die Z Position der Kamera
	private const float rotationDamping = 10.0f;			// ???
	private const float cameraShakingBoundary = 5.0f;		// Grenzwert des Wackelns
	private const float accelarationPositionChange = 3.0f;	// ???

//----------------------------------------------------------------------------------------------------------------------#START
	void Start() {
		// Init the class variables
		cameraShakingState = 0;
		cameraIsShaking = false;
		timerCameraShaking = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationCameraShaking];
	}

//----------------------------------------------------------------------------------------------------------------------#UPDATE
    void Update()
    {
        //Debug.Log("Kamerapos.: " + transform.position);
        wantedPosition = target.TransformPoint(0, height, -distance);
        Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        wantedPosition = target.TransformPoint(0, height, -distance);

		if (Need.ball_direction == 0)
        {
            //target.TransformPoint(150, 0, 0);
            wantedPosition.x = transformX;
            wantedPosition.y += transformY;
            wantedPosition.z += transformZ;
            wantedPosition.z += 100;
            wantedPosition.x += 100;

			// mache weiße Rückwand usnichtbar (right)
        }

		if (Need.ball_direction == 1)
        {
            //target.TransformPoint(-150, 0, 0);
            wantedPosition.x = -transformX;
            wantedPosition.y += transformY;
            wantedPosition.z += transformZ;
            wantedPosition.x -= 100;
            wantedPosition.z += 100;

			// mache schwarze Rückwand unsichtbar (left)
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping * accelarationPositionChange);
        
        if (ItemHandler.eventFlags[1])
        {
            if(!cameraIsShaking)
            {
                Debug.Log("EVENT: Shaking beginnt. Kameraposition: " + transform.position);
                ItemHandler.userReadableNameCurrentActiveEvent = "EARTHQUAKE! EVERYONE TAKE SHELTER";
                

                cameraIsShaking = true;
            }
            if(cameraShakingState == 0)
            {
                shakingVector = new Vector3( Random.Range(-cameraShakingBoundary, cameraShakingBoundary),
                                            Random.Range(-cameraShakingBoundary, cameraShakingBoundary),
                                            Random.Range(-cameraShakingBoundary, cameraShakingBoundary));
                transform.position += shakingVector;
                cameraShakingState = 1;
            }

            if(cameraShakingState == 1)
            {
                transform.position -= shakingVector;
                cameraShakingState = 0;
            }
          
            timerCameraShaking -= Time.deltaTime;
            if(timerCameraShaking < 0.0f)
			{
				Need.isItem = false;
                ItemHandler.eventFlags[1] = false;
                timerCameraShaking = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationCameraShaking];
                ItemHandler.userReadableNameCurrentActiveEvent = "";
                Debug.Log("EVENT: Beende Shaking. Kameraposition: " + transform.position);
            }

        }
	}
}

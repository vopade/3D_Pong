using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disco : MonoBehaviour {

	// Use this for initialization
	private float rotRange = 40; // Range: -0.4 - 0.4
	private byte reduceFactor = 130;
	public static bool isDisco = false;
	private bool lockKeyPress = false;
	private const byte DEBOUNCE = 10;
	private byte timerLockKeyQ = DEBOUNCE;
	//private bool manualEventActivationEnabled = false;
	//private bool manualEventActivationEnabledLock = false;
	//private byte manualEventActivationEnabledLockTimer = DEBOUNCE;
	[SerializeField]
	public AudioClip sdf;
	private const byte lightintensity = 6;
	GameObject toplight, pointLight;
	EventActivation act;
	GameObject audioNormal;
	AudioSource sourceNormal;
	GameObject audioDisco;
	AudioSource sourceDisco;
	AudioSource tmp;

	void Start () 
	{
		audioNormal = GameObject.FindGameObjectWithTag ("backgroundmusic");
		sourceNormal = audioNormal.GetComponent<AudioSource> ();
		audioDisco = GameObject.FindGameObjectWithTag ("backgroundmusicDisco");
		sourceDisco = audioDisco.GetComponent<AudioSource> ();
		toplight = GameObject.FindGameObjectWithTag ("DiscolightLookatTop");
		toplight.GetComponent<Light> ().intensity = 0;
		pointLight = GameObject.FindGameObjectWithTag ("Pointlight");
		act = new EventActivation ();
		foreach (GameObject ele in GameObject.FindGameObjectsWithTag ("Discolight")) // schwarz, p2
		{ 
			Light light = ele.GetComponent<Light> ();
			light.intensity = 0;
		}	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//Debug.Log (pointLight.GetComponent<Light> ().intensity);
		if (!lockKeyPress)
		{
			isDisco = (Input.GetKey (KeyCode.Keypad0)) ? (isDisco = !isDisco): isDisco;
			if(Input.GetKey (KeyCode.Keypad1))
				ItemHandler.eventFlags [0] = true;
			if(Input.GetKey (KeyCode.Keypad2))
				ItemHandler.eventFlags [1] = true;
			if(Input.GetKey (KeyCode.Keypad3))
				ItemHandler.eventFlags [3] = true;
			if(Input.GetKey (KeyCode.Keypad4))
				ItemHandler.eventFlags [4] = true;
			if(Input.GetKey (KeyCode.Keypad5))
				ItemHandler.eventFlags [5] = true;
			if(Input.GetKey (KeyCode.Keypad6))
				ItemHandler.eventFlags [6] = true;
			if(Input.GetKey (KeyCode.Keypad7))
				ItemHandler.eventFlags [7] = true;
			if(Input.GetKey (KeyCode.Keypad8))
				ItemHandler.eventFlags [8] = true;
			if(Input.GetKey (KeyCode.Keypad9))
				ItemHandler.eventFlags [9] = true;

			lockKeyPress = true;
		}

		if (lockKeyPress) 
		{
			timerLockKeyQ--;
			if (timerLockKeyQ <= 0) 
			{
				lockKeyPress = false;
				timerLockKeyQ = DEBOUNCE;
			}
		}


		/*
		if (manualEventActivationEnabled && !manualEventActivationEnabledLock) 
		{
			manualEventActivationEnabledLock = true;
			if (Input.GetKey (KeyCode.Keypad0)) 
			{
				Disco.isDisco = !Disco.isDisco;
			}
			if (Input.GetKey (KeyCode.Keypad0)) 
			{
			}
		}
		if (manualEventActivationEnabledLock) 
		{
			manualEventActivationEnabledLockTimer--;
			if (manualEventActivationEnabledLockTimer <= 0) 
			{
				manualEventActivationEnabledLock = false;
				manualEventActivationEnabledLockTimer = DEBOUNCE;
			}
		}
		*/

		if (isDisco) 
		{
			sourceNormal.mute = true;


			sourceDisco.mute = false;
			//source.clip = Resources.Load("Disco Century") as AudioClip;

			pointLight.GetComponent<Light> ().intensity = 0.000001f;//0.4f;

			// DISCO DISCO PARTY PARTY
			foreach (GameObject ele in GameObject.FindGameObjectsWithTag ("Discolight")) { // schwarz, p2
				Light light = ele.GetComponent<Light> ();
				light.intensity = lightintensity;
				light.transform.Rotate (generateRandomRotation ());
				toplight.GetComponent<Light> ().intensity = lightintensity;
			}	
			toplight.transform.Rotate (new Vector3 (-0.08f, 0, 0));
		} else 
		{
			pointLight.GetComponent<Light> ().intensity = 4f;//1.75f;	
			foreach (GameObject ele in GameObject.FindGameObjectsWithTag ("Discolight")) { // schwarz, p2
				Light light = ele.GetComponent<Light> ();
				light.intensity = 0;
				light.transform.Rotate (generateRandomRotation ());
				toplight.GetComponent<Light> ().intensity = 0;
				sourceNormal.mute = false;
				sourceDisco.mute = true;
			}	
		}
			
	}

	private Vector3 generateRandomRotation()
	{
		// Farbkegel drehen sich sehr regelmäßig
		return new Vector3 ((Random.Range (0, rotRange) / reduceFactor / Random.Range (20, 60)), // / Random.Range (20, 60) unregelmäßigkeit erhöhen
							(Random.Range (0, rotRange) / reduceFactor / Random.Range (2, 4)),
							(Random.Range (-rotRange, rotRange)/ (reduceFactor+0) / Random.Range (2, 4)));

		
		/*
		int tmp = Random.Range(0, 2);
		if (tmp == 0) 
		{
			return new Vector3 ((Random.Range (0, rotRange) / reduceFactor / Random.Range (20, 60)), // / 10 = unregelmäßigkeit reinbringen
				(Random.Range (0, rotRange) / reduceFactor / Random.Range (2, 4)),
				(Random.Range (0, rotRange) / reduceFactor));
		}
		else 
		{
			return new Vector3 ((Random.Range (0, rotRange) / reduceFactor / Random.Range (20, 60)), // / 10 = unregelmäßigkeit reinbringen
							  	 0,
								(Random.Range (0, rotRange) / reduceFactor));
		}
		*/
	}
}

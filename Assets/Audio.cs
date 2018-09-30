using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour 
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the sounds
	private static byte NUM_AUDIO;		// Anzahl der Sounds

	// Def class variables for the sounds
	public AudioClip[] clips;			// Array der Audioclips
	public AudioSource source;			// Quelle der Audios
	public static bool[] audioFlags;	// Ist ein Audio gesetzt
	public enum AudioFiles {			// Name der Audios
		ItemHit,
		ItemBlinking,
		ItemExpired,
		Menu,
		GameStart,
		GameEnd,
		BallBounce
	};

//----------------------------------------------------------------------------------------------------------------------#START
	void Start() {
		// Init the class variables
		NUM_AUDIO = 7;
		audioFlags = new bool[NUM_AUDIO];
	}

//----------------------------------------------------------------------------------------------------------------------#UPDATE
    void Update () 
    {
        for (int i = 0; i < NUM_AUDIO; i++)
        {
            if (audioFlags[i])
            {
                source.clip = clips[i];
                audioFlags[i] = false;
                source.Play();
            }
        }
        //source.PlayOneShot(clip);
	}
}

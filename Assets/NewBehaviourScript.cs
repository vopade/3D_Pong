using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the newbehaviourscript
	private string anzeige;				// Der Anzeigestring
	private bool textIsVisible;			// Der Status des Anzeigefelds
	private float timerDisplay;			// Der Zeitgeber der Anzeigetextes
	private float timerNothingHappend;	// Der Zeitgeber des Bildes

	// Def class variables for the newbehaviourscript
	public Text fuckingshit;			// Der Text, der momentan die Icons ersetzt
    public RawImage nothingHappendIm;	// Das Bild, wenn nichts passiert

//----------------------------------------------------------------------------------------------------------------------#START
    void Start()
	{
		// Init the class variables
		anzeige = "";
		timerDisplay = 3.0f;
		textIsVisible = false;
		nothingHappendIm.enabled = false;

		// Raw Image (Script) in New beahavouir Script 8Script) Nothing happend Im ziehen
		timerNothingHappend = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationNothingHappend];
    }
    
//----------------------------------------------------------------------------------------------------------------------#FIXEDUPDATE
    void FixedUpdate()
    {
        // userReadableNameCurrentActiveEvent wird konstant überschrieben, dreckiger workaround:
        //fuckingshit.text = ItemHandler.userReadableNameCurrentActiveEvent;
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.BallDisappear])
            anzeige = "Where did the ball go?";
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.BallDirectionInversion])
            anzeige = "OMG! Look at the Direction!";
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.BallRespawn])
            anzeige  ="$h!t. Where's the Ball?";
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.BallSpeedUp])
            anzeige = "WOW! That is faaaaaaaast!";
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.CameraShaking])
            anzeige = "EARTHQUAKE! EVERYONE TAKE SHELTER";
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.PanelInversion])
            anzeige = "HELP! My Panel is broken!";
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.PanelShrinking])
            anzeige = "ALARM! ALARM!";
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.PanelSlowdown])
            anzeige = "C'mon MOOOVE you lazy Donkey!";
        //fuckingshit.text = anzeige;

        /*if(anzeige != "")
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay <= 0.0f)
			{
                fuckingshit.text = "";
                anzeige = "";
                timerDisplay = 3.0f;
            }
        }*/
        
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.NothingHappend])
        {
            if (!textIsVisible)
            {
                nothingHappendIm.enabled = true;
                ItemHandler.userReadableNameCurrentActiveEvent = "TROLOLOLOLOL";
                Debug.Log("EVENT: Nothing happend. Bild wird angezeigt");
                textIsVisible = true;
            }
            timerNothingHappend -= Time.deltaTime;

            if (timerNothingHappend <= 0.0f)
			{
				Need.isItem = false;
                nothingHappendIm.enabled = false;
                ItemHandler.userReadableNameCurrentActiveEvent = "";
                textIsVisible = false;
                ItemHandler.eventFlags[(int)ItemHandler.Events.NothingHappend] = false;
                timerNothingHappend = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationNothingHappend];
                Debug.Log("EVENT: Nothing happened vorbei");
            }
        }
    }
}

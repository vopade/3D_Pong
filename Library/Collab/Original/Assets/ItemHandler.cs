/*
 * Reference for itemNumber
 * 0: DONE: speed up ball, Wkeit: 10%
 * 1: DONE: Shaking , Wkeit: 25%
 * 2: Nichts passiert (Bild poppt auf), Welche Kameraeinstellung?, Wkeit: 10%
 *          Einfach Bild einem Objekt übergeben
 * 3: DONE: Steuerung des Schlägers des Angegriffenen invertiert, Wkeit: 5%
 * 4: DONE Schläger des Angegriffenen werden langsamer, Wkeit: 5%
 * 5: DONE: Ball verschwinden lassen für bestimmte Zeit, Wkeit: 10%
 * 6: DONE: Richtung des Balls invertiert sich bei Kollision, Wkeit: 10%
 * 7: DONE: spawned komplett neu (inkl. zufällige Richtung), Wkeit: 20%
 * 8: BUGGY: Schläger werden kleiner, Wkeit: 5%
 * x: Irgendwas mit Licht (Einfallwinkel etc.)
 *           
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour
{
    private byte eventNumber = 0;
    private byte cumulativeEventprobabilities = 0;
    private const byte NUMITEMS = 10;
    private float timerItemDestroy = 60.0f;
    private const byte blinkDelay = 40;
    private byte cnt = 0;
    private byte cnt2 = blinkDelay / 2;
    private const float itemBlinkDuration = 10.0f;
    //private byte[] cumulativeEventPropabilities = new byte[] {10, 35, 45, 50, 55, 65, 75, 95, 100};
    private byte[] eventPropabilities = new byte[] { 10, 20, 10, 5, 5, 10, 10, 15, 5,  10};
    private Vector3 defaultLocalScale;
    public static string userReadableNameCurrentActiveEvent = "";
    public static bool[] eventFlags = new bool[NUMITEMS];
    public static float[] eventSpecifications = new float[] { 2.0f, 2.0f, 2.0f, 7.0f, 5.0f, 5.0f, 1.7f, 1.0f, 4.0f, 2.0f };
    public enum Events
    {
        BallSpeedUp, CameraShaking, NothingHappend, PanelInversion, PanelSlowdown,
        BallDisappear, BallDirectionInversion, BallRespawn, PanelShrinking, Pong2D
    };
    public enum EventSpecifications
    {
        durationSpeedup, durationDisappearance, timeoutInvert, durationCameraShaking, durationPanelSlowdown,
        durationPanelInvert, panelSlowdownFactor, durationPanelShrink, panelShrinkingFactor, durationNothingHappend
    };
	private RawImage icon1;
	private RawImage icon2;
	public Texture ball;
	public Texture panel;
	public Texture video;
	public Texture invers;
	public Texture smaller;
	public Texture faster;
	public Texture shaking;
	public Texture nothing;
	public Texture slower;
	public Texture newBall;
	public Texture notseeing;
	public Texture empty;

	void Awake() {
		StartCoroutine (waitfor(0));
	}

    // Gültigkeitsbereich: panelSlowdownFactor [1, 15]
    void Start()
    {

        defaultLocalScale = this.transform.localScale;
        /*
        ball = Resources.Load("/ball") as Texture;  
		panel = Resources.Load("panel") as Texture;
		video = Resources.Load("camera") as Texture;
		invers = Resources.Load("invers") as Texture;
		smaller = Resources.Load("smaller") as Texture;
		faster = Resources.Load("faster") as Texture;
		shaking = Resources.Load("shaking") as Texture;
		nothing = Resources.Load("nothing") as Texture;
		slower = Resources.Load("slower") as Texture;
		newBall = Resources.Load("new") as Texture;
		notseeing = Resources.Load("no-seeing") as Texture;
		empty = Resources.Load("empty") as Texture;
        */
    }

    void FixedUpdate()
    {
        timerItemDestroy -= Time.deltaTime;
        if (timerItemDestroy <= itemBlinkDuration && timerItemDestroy > 0.0f)
        {
            if (cnt2 % blinkDelay == 0)
            {
                //Debug.Log("An");
                this.transform.localScale = defaultLocalScale;
                cnt2 = 0;

            }
            if (cnt % blinkDelay == 0)
            {
                Audio.audioFlags[(int)Audio.AudioFiles.ItemBlinking] = true;
                //Debug.Log("Aus");
                this.transform.localScale = new Vector3(0f, 0f, 0f);
                cnt = 0;
            }
            cnt++;
            cnt2++;
            //itemVisible = !itemVisible;
        }

        if (timerItemDestroy <= 0.0f)
        {
            Debug.Log("ITEM: Destroyed");
            Audio.audioFlags[(int)Audio.AudioFiles.ItemExpired] = true;
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other) // other ist Kugel, this is kiste
    {
        /* Es werden insgesamt 5 Kollisionen detektiert, da der Collider der Kiste 
         * aus 5 Elementen besteht
         * 
         * Nur Kollisionen zwischen Ball und Item abfragen. Unsauber, da Kriterium für Ball name ist. 
         * Typ vom Ball ist SphereCollider, davon kann es aber mehrere geben
         */

        if (other.name == "Ball" && this is ItemHandler)
        {
            /*Debug.Log(  "COLLISION: Other: " + other.name + "(Type)" + other.GetType() + 
                        "This: " + this.name + "(Type)" + this.GetType());*/

            /* Wahrscheinlichkeitssystem:
             * Verteilung der Wahrscheinlichkeiten der Events: s.o. 
             * Werte werden addiert und dann wird abgefrat, ob Wert kleiner ist. 
             */

            eventNumber = (byte)Random.Range(0, 100);
            cumulativeEventprobabilities = eventPropabilities[0];
            for (int i = 0; i < NUMITEMS; i++)
            {
                if (eventNumber < cumulativeEventprobabilities)
                {
                    //eventFlags[(byte)Random.Range(0, (int)NUMITEMS)] = true; // Random.Range inkludiert auch obere Grenze
                    eventFlags[(int)ItemHandler.Events.BallSpeedUp] = true; // Testen
                    Debug.Log("EVENT: Event Match: " + i);
                    break;
                }
                cumulativeEventprobabilities += eventPropabilities[i];
            }
            Audio.audioFlags[(int)Audio.AudioFiles.ItemHit] = true;
            Destroy(this.gameObject);
        }

		//showIcon ();
        // weiter nach unten und nach links
    }

	void showIcon() {
		if (Need.ball_direction == 0) {
			icon1 = GameObject.Find ("IconsPlayer_1").transform.GetChild (0).GetComponent<RawImage> ();
			icon2 = GameObject.Find ("IconsPlayer_1").transform.GetChild (1).GetComponent<RawImage> ();
            
			if(eventFlags[(int)ItemHandler.Events.BallSpeedUp]) {

                icon1.texture = ball;
				icon2.texture = faster;
			}
			if(eventFlags[(int)ItemHandler.Events.BallRespawn]) {
				icon1.texture = ball;
				icon2.texture = newBall;
			}
			if(eventFlags[(int)ItemHandler.Events.BallDisappear]) {
				icon1.texture = ball;
				icon2.texture = notseeing;
			}
			if(eventFlags[(int)ItemHandler.Events.BallDirectionInversion]) {
				icon1.texture = ball;
				icon2.texture = invers;
			}
			if(eventFlags[(int)ItemHandler.Events.CameraShaking]) {
				icon1.texture = video;
				icon2.texture = shaking;
			}
			if(eventFlags[(int)ItemHandler.Events.NothingHappend]) {
				icon1.texture = nothing;
				icon2.texture = empty;
			}
			if(eventFlags[(int)ItemHandler.Events.PanelInversion]) {
				icon1.texture = panel;
				icon2.texture = invers;
			}
			if(eventFlags[(int)ItemHandler.Events.PanelShrinking]) {
				icon1.texture = panel;
				icon2.texture = smaller;
			}
			if(eventFlags[(int)ItemHandler.Events.PanelSlowdown]) {
				icon1.texture = panel;
				icon2.texture = slower;
			}
			waitfor(3);
			//icon1.texture = empty;
			icon2.texture = empty;
		} else {
			icon1 = GameObject.Find ("IconsPlayer_1").transform.GetChild (0).GetComponent<RawImage> ();
			icon2 = GameObject.Find ("IconsPlayer_1").transform.GetChild (1).GetComponent<RawImage> ();
			if(eventFlags[(int)ItemHandler.Events.BallSpeedUp]) {
				icon1.texture = ball;
				icon2.texture = faster;
			}
			if(eventFlags[(int)ItemHandler.Events.BallRespawn]) {
				icon1.texture = ball;
				icon2.texture = newBall;
			}
			if(eventFlags[(int)ItemHandler.Events.BallDisappear]) {
				icon1.texture = ball;
				icon2.texture = notseeing;
			}
			if(eventFlags[(int)ItemHandler.Events.BallDirectionInversion]) {
				icon1.texture = ball;
				icon2.texture = invers;
			}
			if(eventFlags[(int)ItemHandler.Events.CameraShaking]) {
				icon1.texture = video;
				icon2.texture = shaking;
			}
			if(eventFlags[(int)ItemHandler.Events.NothingHappend]) {
				icon1.texture = nothing;
				icon2.texture = empty;
			}
			if(eventFlags[(int)ItemHandler.Events.PanelInversion]) {
				icon1.texture = panel;
				icon2.texture = invers;
			}
			if(eventFlags[(int)ItemHandler.Events.PanelShrinking]) {
				icon1.texture = panel;
				icon2.texture = smaller;
			}
			if(eventFlags[(int)ItemHandler.Events.PanelSlowdown]) {
				icon1.texture = panel;
				icon2.texture = slower;
			}
			waitfor(3);
			icon1.texture = empty;
			icon2.texture = empty;
		}
	}

	private IEnumerator waitfor(int i) {
		yield return i;
	}
}
